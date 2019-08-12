using UnityEngine;
using System.Collections;

public class GunScript : MonoBehaviour {

	private bool shoot;  //Bool to know if we're shooting

	public bool hasAmmo; //Bool to know if we've got ammo.

	public bool hasGun; //Bool to know if we have a gun.

	public GameObject bullet; //This is the bullet prefab, drag it into here publicly.


	void Start () {

		shoot = false;              //We have nothing and are not shooting on start up.

		hasAmmo = false;             // hasAmmo and hasGun are set to true from the Hero script when you pick up the gun or ammo.

		hasGun = false;

	}


	void Update () {

		shoot = Input.GetKeyDown (KeyCode.Z);   //Set space bar to be shoot. Whatever it is must be the same key as the attack in the hero script. I tried simply sending a bool
	}												// to this script to set shoot to true, but it sets it true forever until stopped. This is a quick and easy way.

	void FixedUpdate(){

		if (shoot && hasAmmo && hasGun) {   //If you shoot, have ammo and have a gun.

			GameObject aBullet = (GameObject)Instantiate (bullet, transform.position, transform.rotation);  //Create a bullet from the bullet prefab/

			if (transform.parent.localScale.x < 0) {

				Vector3 bulletScale = aBullet.transform.localScale;
				bulletScale.x *= -1;                                     //Direct it in the direction of it's parent.
				aBullet.transform.localScale = bulletScale;
			}

			aBullet.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (50 * transform.parent.localScale.x, 0), ForceMode2D.Impulse);  //Apply this force to it
		}
	}
}
