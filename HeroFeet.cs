using UnityEngine;
using System.Collections;

public class HeroFeet : MonoBehaviour {

	public GameObject hero; //We need a refernce to our Hero, drag him into this publicly.


	void Start () {

	
	}


	void FixedUpdate () {


	}


	void OnTriggerEnter2D(Collider2D other)
	{

		hero.GetComponent<Hero> ().inJump = false;      //If the feet touch something, set the inJump bool in the Hero object to false.
		hero.GetComponent<Animator>().SetBool("jump", false);

	}



}
