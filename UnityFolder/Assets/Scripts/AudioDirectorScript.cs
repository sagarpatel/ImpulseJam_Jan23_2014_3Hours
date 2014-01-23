using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]

public class AudioDirectorScript : MonoBehaviour 
{
	public bool requestPending;
	public bool isAuthget = false;
	public string[] devicesArray;
	public int currentlySelectedDeviceIndex = 0;

	public AudioSource currentAudioSource;

	public float[] spectrumDataArray;

	public float fftSum = 0;

	// Use this for initialization
	void Start () 
	{
		Debug.Log("AUDIODIRECTOR START");

		StartCoroutine("RequestAuthorize");

		currentAudioSource = GetComponent<AudioSource>();
		spectrumDataArray = new float[32];
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
		currentAudioSource.mute = false;
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
			string fftSumString = "fftSum: " + fftSum.ToString();
			GUI.Label(new Rect(500,0,600,20), fftSumString);

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

		//fftSum = 0;
		foreach(float fftValue in spectrumDataArray)
		{
			fftSum += fftValue;
		}

		//		Debug.Log("GettinDatData");
	}

}
