using UnityEngine;
using System.Collections;

public class BasicEnemyScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{
        if (coll.gameObject.tag == "Bullet")
        {
        	Destroy(coll.gameObject, 0.0f); // destroy bullet
        	Destroy(gameObject, 0.9f); // self destruct
        }

    }
}
