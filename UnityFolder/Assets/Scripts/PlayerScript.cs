using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour 
{

	public GameObject bulletPrefab;

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
		HandleFiring();
	
	}

	void HandleMovement()
	{
		Vector3 tempPosition = transform.position;
		tempPosition.x +=  Input.GetAxis("Horizontal") * hSpeedScale * Time.deltaTime;
		tempPosition.y +=  Input.GetAxis("Vertical") * vSpeedScale * Time.deltaTime;
		transform.position = tempPosition;
	}

	void HandleFiring()
	{
		if(Input.GetButtonUp("Fire1"))
		{
			Instantiate(bulletPrefab, transform.position, Quaternion.identity);
		}

	}

}
