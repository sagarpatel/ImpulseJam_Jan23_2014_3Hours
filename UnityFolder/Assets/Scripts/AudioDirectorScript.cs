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
	public AudioClip currentAudioClip;

	public float[] spectrumDataArray;

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
		
			}


			HandleAudioData();
		}
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
			//InitMicrophone();
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

		currentAudioClip = Microphone.Start(devicesArray[currentlySelectedDeviceIndex], true, 1, 44100);
		currentAudioSource.clip = currentAudioClip;

		currentAudioSource.GetSpectrumData(spectrumDataArray, 0, FFTWindow.BlackmanHarris);

		//		Debug.Log("GettinDatData");
	}

}
