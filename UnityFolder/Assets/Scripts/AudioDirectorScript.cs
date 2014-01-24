using UnityEngine;
using System.Collections;

/*
Copying over some things from Frequency Domain to save time.
They really are truely terrible things that need to be fixed, but I really need to get this done before starting GGJ14
*/

[RequireComponent (typeof (AudioSource))]

public class AudioDirectorScript : MonoBehaviour 
{
	public bool requestPending;
	public bool isAuthget = false;
	public string[] devicesArray;
	public int currentlySelectedDeviceIndex = 0;

	public AudioSource currentAudioSource;

	public float[] spectrumDataArray = new float[1024];
	float[] pseudoLogArray = new float[100];
	public int sampleStartIndex= 0;
	public int[] samplesPerDecadeArray = new int[10];


	public float rScale = 1.0f;
	public float bScale = 1.0f;
	public float gScale = 1.0f;
	public Color calculatedRGB = new Color();

	public float fftSum = 0;

	// Use this for initialization
	void Start () 
	{
		Debug.Log("AUDIODIRECTOR START");

		StartCoroutine("RequestAuthorize");

		currentAudioSource = GetComponent<AudioSource>();
		//spectrumDataArray = new float[32];
		spectrumDataArray[1] = 17; // such debuging
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(isAuthget)
		{	
			if(Input.GetKeyUp("e") || Input.GetKeyUp("e") )
			{
				
				if(currentlySelectedDeviceIndex >= devicesArray.Length -1)
					currentlySelectedDeviceIndex = 0;
				else
					currentlySelectedDeviceIndex += 1;

				SetupAudioSourceWithMicrophone(currentlySelectedDeviceIndex);
			
			}


			HandleAudioData();
		}
	}

	void SetupAudioSourceWithMicrophone(int deviceIndex)
	{
		currentAudioSource.loop = true;
		currentAudioSource.volume = 1.0f;
		currentAudioSource.mute = true;
		currentAudioSource.playOnAwake = true;

		currentAudioSource.clip = Microphone.Start(devicesArray[deviceIndex], true, 1, 44100);
		currentAudioSource.Play();
	}

	public IEnumerator RequestAuthorize()
	{
		requestPending = true;
		yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
		if (Application.HasUserAuthorization(UserAuthorization.Microphone))
		{	
			Debug.Log("MICROPHONE AUTH GET!");
			devicesArray = Microphone.devices;	
			isAuthget = true;
			requestPending = false;
			SetupAudioSourceWithMicrophone(currentlySelectedDeviceIndex);
		}

		Debug.Log(Microphone.devices.Length);
		
		foreach (string device in Microphone.devices) 
		{
            Debug.Log("Name: " + device);
        }
	}



	void OnGUI()
	{
		if(isAuthget)
		{	

			//string fftSumString = "fftSum: " + fftSum.ToString();
			//GUI.Label(new Rect(500,0,600,20), fftSumString);

	    	GUI.Label(new Rect(0,0,200,20),"List of available devices:");
	    	GUI.Label(new Rect(200,0,200,20),"Press e to switch devices");

			float yPos = 0;
	    	float offset = 10.0f;
			foreach(string device in devicesArray)
	    	{
	    		GUI.Label(new Rect(10, 20 + yPos, 500, 20), device);
	    		yPos += offset;
	    	}

			GUI.Label(new Rect(0, 40 + yPos,200,20),"Currently selected device:");
			GUI.Label(new Rect(10, 60 + yPos,500,20), devicesArray[currentlySelectedDeviceIndex]);
		}
		else
		{
			GUI.Label(new Rect(0,0,1000,20),"FAILED TO GET AUTHENTIFICATION FOR MICROPHONE INPUT");			
		}

	}

	void HandleAudioData()
	{
		currentAudioSource.GetSpectrumData(spectrumDataArray, 0, FFTWindow.BlackmanHarris);

		// cleanup pseudolog array first
		for(int i = 0; i < pseudoLogArray.Length; i++)
			pseudoLogArray[i] = 0;
		

		// doing the pseudo log scale
		int decadeIndex = 0;
		int fftSampleCounter = sampleStartIndex;//0;
		for(int i = 0; i < pseudoLogArray.Length; i++)
		{
			if( i != 0 && i%10 == 0)
				decadeIndex++;

			for(int j = 0; j < samplesPerDecadeArray[decadeIndex]; j++ )
			{
				pseudoLogArray[i] += spectrumDataArray[fftSampleCounter] ;
				fftSampleCounter++;
			}
		}

		CalculateRBG();

	}

	void CalculateRBG()
	{
		/// Hanlde RGB calculation

		// linear scaling model (triangular)
		// assuming a pseudo log array of size 100, cuz I'm lazy. It's easy to generalize, but don't feel like doing it now
		
		// Red (full), linear slope down, max 1.0
		// sine sweek test shows linear 1:1 dropoff is too weak, need to make slope less steep
		float tempR = 0;
		for( int i = 0; i < 40; i++ )
		{
			tempR += pseudoLogArray[i] * Mathf.Clamp( (40.0f - 0.75f*(float)i)/40.0f ,0,1) ; // weighted according to position on slope
		}
	
		// Green (first half), linear slope up, max 2/3 --> 0.67 (fromarea under curve calcuation)
		// sine sweek test shows linear 1:1 dropoff is too weak, need to make slope less steep
		float tempG  = 0;
		for( int i = 20; i < 50; i++ )
		{
			tempG += pseudoLogArray[i] * Mathf.Clamp( 0.6f * ( (1.6f*(float)i - 20.0f) )/30.0f ,0,1);
		}

		// Green (second half), linear slope down
		for( int i = 50; i < 80; i++ )
		{
			tempG += pseudoLogArray[i] * Mathf.Clamp( 0.6f * (30.0f - 0.8f*(float)(i-50) )/30.0f ,0,1);
		}


		// Blue (full), linear slope up
		// blue is to powerful, going from 1.5f to 1.0f
		float tempB = 0;
		for( int i = 60; i < 100; i++)
		{
			tempB += pseudoLogArray[i] * Mathf.Clamp( ( (1.0f*(float)i - 60.0f) )/40.0f , 0,1);
		}
	

		calculatedRGB = new Color( tempR * rScale, tempG * gScale, tempB * bScale, 1.0f);
	}

}
