using UnityEngine;
using System.Collections;

public class BasicBulletScript : MonoBehaviour 
{
	public float bulletSpeedScale = 1.0f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 tempPosition = transform.position;
		tempPosition.y += bulletSpeedScale * Time.deltaTime;
		transform.position = tempPosition;
	}



}
