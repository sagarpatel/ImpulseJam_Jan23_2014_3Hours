using UnityEngine;
using System.Collections;

public class WeaponsArrayScript : MonoBehaviour 
{

	GameObject audioDirector;
	public Color calculatedRGB;

	public GameObject lowBulletPrefab;
	public GameObject midBulletPrefab;
	public GameObject highBulletPrefab;

	GameObject lowWeapon;
	GameObject midWeapon;
	GameObject highWeapon;

	public float weaponsScale = 1.0f;

	// Use this for initialization
	void Start () 
	{
		audioDirector = GameObject.FindWithTag("AudioDirector");	

		lowWeapon = (GameObject)Instantiate(lowBulletPrefab, new Vector3(), Quaternion.identity);
		midWeapon = (GameObject)Instantiate(midBulletPrefab, new Vector3(), Quaternion.identity);
		highWeapon = (GameObject)Instantiate(highBulletPrefab, new Vector3(), Quaternion.identity);

		lowWeapon.transform.parent = transform;
		midWeapon.transform.parent = transform;
		highWeapon.transform.parent = transform;

		lowWeapon.transform.localPosition = new Vector3(-3, 0, 0);
		midWeapon.transform.localPosition = new Vector3(0, 0, 0);
		highWeapon.transform.localPosition = new Vector3(3, 0, 0);

		// deactivate the bullet script on weapons so that they don't fly away
		lowWeapon.GetComponent<BasicBulletScript>().enabled = false;
		midWeapon.GetComponent<BasicBulletScript>().enabled = false;
		highWeapon.GetComponent<BasicBulletScript>().enabled = false;

		lowWeapon.tag = "Weapon";
		midWeapon.tag = "Weapon";
		highWeapon.tag = "Weapon";

	}
	
	// Update is called once per frame
	void Update () 
	{
		calculatedRGB = audioDirector.GetComponent<AudioDirectorScript>().calculatedRGB;
		HandleWeaponScales();
		HandleWeaponsColors();
	
	}

	void HandleWeaponScales()
	{

		lowWeapon.transform.localScale = calculatedRGB.r * weaponsScale * lowBulletPrefab.transform.localScale;
		midWeapon.transform.localScale = calculatedRGB.g * weaponsScale * midBulletPrefab.transform.localScale;
		highWeapon.transform.localScale = calculatedRGB.b * weaponsScale *  highBulletPrefab.transform.localScale;

	}

	void HandleWeaponsColors()
	{
		lowWeapon.renderer.material.color = new Color(calculatedRGB.r, 0, 0, calculatedRGB.r);
		midWeapon.renderer.material.color = new Color(0, calculatedRGB.g, 0, calculatedRGB.g);
		highWeapon.renderer.material.color = new Color(0, 0, calculatedRGB.b, calculatedRGB.b);
	}

}
