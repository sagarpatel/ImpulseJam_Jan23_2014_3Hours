using UnityEngine;
using System.Collections;

public class AudioDirectorScript : MonoBehaviour 
{
	public bool requestPending;

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
			//InitMicrophone();
		}

		Debug.Log(Microphone.devices.Length);	
		foreach (string device in Microphone.devices) 
		{
            Debug.Log("Name: " + device);
        }
	}

}
