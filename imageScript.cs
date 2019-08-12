using UnityEngine;
using System.Collections;
using UnityEngine.UI;       //Remember, this is needed for working with the canvas!

public class imageScript : MonoBehaviour {

	public Sprite fist;      //The GUI image for the fist when you have no gun

	public Sprite gun;   //The GUI image for the gun when you have the gun.

	private Image theImage;  //This is the blank image we want to toggle between being a fist or gun.

	public bool hasGun; //We have to know if we have a gun or not.



	void Start () {

		hasGun = false;    //We don't have a gun at the start. This is toggled by testing if "hasGun" bool in the hero script is true or false.

		theImage = GetComponent<Image> ();   //So we get the image component from the inspector.

		theImage.sprite = fist;  //And set the sprite to be be a fist.

	}
	

	void Update () {

		if (GameObject.Find ("Hero").GetComponent<Hero> ().hasGun == true) {   //This is a way of getting or seting a bool from another script. This finds the Game object called "Hero",
			hasGun = true;													   // i dont think you even need to tag it,	and get the component "Hero" in it, which is the name of the SCRIPT.
		} 																	   // //This is confusing, so the hero script should be named "hero Script" to make it easier. The script compoent is
																			  //named after the script's name. So, if i had named it correctly, the code would be:
		else {																  //GameObject.Find ("Hero").GetComponent<HeroScript> ().hasGun == true;					
			hasGun = false;												
		}																
																		//So, we test if the "hasGun" bool in THAT script is true, and if it is, we set the hasGun bool in THIS script to true.
																		// And if it's not, we set it to false.
																			
		if(hasGun)
		{
												//If hasGun is true, then set the image to the gun.
			theImage.sprite = gun;

		} else {							//If hasGun is false, set the image to the fist.

			theImage.sprite = fist;
		}
	
	}
}
