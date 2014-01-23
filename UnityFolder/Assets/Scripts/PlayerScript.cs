using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{

	public float hSpeedScale = 1.0f;
	public float vSpeedScale = 1.0f;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{

		HandleMovement();
	
	}

	void HandleMovement()
	{
		Vector3 tempPosition = transform.position;
		tempPosition.x +=  Input.GetAxis("Horizontal") * hSpeedScale * Time.deltaTime;
		tempPosition.y +=  Input.GetAxis("Vertical") * vSpeedScale * Time.deltaTime;
		transform.position = tempPosition;
	}

}
