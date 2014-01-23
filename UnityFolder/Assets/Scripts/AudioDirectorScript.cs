using UnityEngine;
using System.Collections;

public class AudioDirectorScript : MonoBehaviour 
{
	public bool requestPending;
	public bool isAuthget = false;
	public string[] devicesArray;

	// Use this for initialization
	void Start () 
	{
		Debug.Log("AUDIODIRECTOR START");

		StartCoroutine("RequestAuthorize");

		
	}
	
	// Update is called once per frame
	void Update () 
	{
	
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
    	//GUI.Label(Rect(0,0,Screen.width,Screen.height),"TEST");
		float yPos = 0;
    	float offset = 10.0f;
		foreach(string device in devicesArray)
    	{
    		GUI.Label(new Rect(10, yPos, 100, 20), device);
    		yPos += offset;
    	}
	}

}
