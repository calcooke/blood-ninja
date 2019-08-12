using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Hero : MonoBehaviour {


	private bool jump, left, right, switchWeapon, esc;   //Bools for keys

	public bool inJump;  //Bool for indicating when in jump

	private bool attack; // Bool for attack (Set to space, needs to be set to something else)

	public new Rigidbody2D rigidbody; 

	private Animator anim;

	private Transform moveHero; //Tranform for changing players position

	private Transform enemyPos; //Transform for enemies position

	public float YJumpForce; //Upward force to apply when jumping, set publicly in inspector.

	public float speed; //Speed to walk at, set publicly in inspector.

	public float XHorizontalForce; // Now redundant, needed for moving with velocity.

	private Vector2 jumpForce; //A vector jumpforce applied to ridigbody when jumping.

	public Vector2 horizontalForce; // Now redundant.

	private int score;

	public float health; //Health for player, set publically. This is a float, so 1 is the max. The reason this is float and not an int, is because the heatlh bar "fill amount"
						 //is determined by a float. 

	float damage = 0.337f; //Damage from enemy, also a float because "health" is a float. Can take 3 hits before health reaches 0.

	private GameObject theGun; //The gun you pick up. I don't think this is actually needed anymore.

	//public GameObject citySkyline; // Throwing this in here to try stop the cityscape flipping when the hero flips, because its a child of the hero!

	private GameObject door; //For the door that you open with the keycard

	private GameObject openDoor; //This is the door object to show when its open

	private AudioSource gunSound;

	private AudioSource hitSound;

	private AudioSource tumbleSound;   //All these are just sounds effects.

	private AudioSource gunClick;

	private AudioSource paper;

	private AudioSource gunShot;

	private AudioSource getAmmo;

	private AudioSource openingDoor;

	public bool hasKey; //Bool to test if you have the key to get through the door.

	public bool hasTicket; //Test if you have a subway ticket.

	public bool hasGun; //Test if you have a gun.

	public bool collectedGun; //Another bool to test if you've collected the gun, so you can't switch weapons without it.

	private bool hasAmmo; //Test if you have ammo.

	public int ammo; //An int for the amount of ammo, can be set publically but is actually set when you pick up some ammo.

	public Image healthBar; //The image for the health bar.

	public Text ammoCount; //The text that shows the ammo count 

	public Text scoreCount; //The text that shows the score;

	public GunScript gunScript;  //This is the script that fires the gun, we need to get it to let it know that we have the gun so it cant fire until we do! The gunScript also has
								// a "hasGun" bool that we can change from here.

	void Start () 
	{
		score = 0;

		jump = left = right = false;

		inJump = false;

		attack = false;

		hasKey = false;     //Everything that needs to be set to false. He isn't juping, attacking, he hasnt a gun, ticket, ammo, etc.

		hasTicket = false;

		hasGun = false;

		collectedGun = false;

		hasAmmo = false;

		rigidbody = GetComponent<Rigidbody2D>();   

		anim = GetComponent<Animator> ();               //Assigning all the variables their component.

		moveHero = GetComponent<Transform> ();

		gunScript = GetComponentInChildren<GunScript> ();

		door = GameObject.FindGameObjectWithTag ("exitDoor");

		openDoor = GameObject.FindGameObjectWithTag ("doorOpen");

		openDoor.SetActive (false);  //Deactivating the open door until we tell it to appear.

		tumbleSound = GameObject.FindGameObjectWithTag ("swoosh").GetComponent<AudioSource> ();

		gunClick = GameObject.FindGameObjectWithTag ("gunClick").GetComponent<AudioSource> ();

		gunShot = GameObject.FindGameObjectWithTag ("gunShot").GetComponent<AudioSource> ();     //Assigning all the sounds their audio. These are not in the Hero object
																							     // so i'm getting them from elsewhere by their tags.
		hitSound = GameObject.FindGameObjectWithTag ("hitSound").GetComponent<AudioSource> ();

		paper = GameObject.FindGameObjectWithTag ("paper").GetComponent<AudioSource> ();

		getAmmo = GameObject.FindGameObjectWithTag ("ammoLoad").GetComponent<AudioSource> ();

		openingDoor = GameObject.FindGameObjectWithTag ("openingDoor").GetComponent<AudioSource> ();

		ammoCount = GameObject.FindWithTag("ammoCount").GetComponent<Text>();

		scoreCount = GameObject.FindWithTag("score").GetComponent<Text>();

	}

	void Update () 
	{

		jump = Input.GetKeyDown(KeyCode.Space);        //This needs to be changed to space for the project.

		attack = Input.GetKeyDown (KeyCode.Z);      //Change this so something else so that jump can be space. Maybe Z?
														// That would give a nice hand position on the keyboard.
		left = Input.GetKey(KeyCode.LeftArrow);

		right = Input.GetKey(KeyCode.RightArrow);

		switchWeapon = Input.GetKeyDown(KeyCode.X);

		esc = Input.GetKeyDown (KeyCode.Escape);

		if (healthBar.fillAmount == 0) {         //If the fill amount on the health bar reaches 0, call the die function. Die is down at the bottom of the code, it just resets the level.

			die ();

		}

		ammoCount.text = ammo.ToString ();     //The ammo count text appears under the gun image. Ammo count is an Int, so needs to be converted to a string for text.

		scoreCount.text = score.ToString();  //Convert the score INT to a string to it can be displayd as text.

		if (score == 70) {

			SceneManager.LoadScene ("End_Scene");
		}
	}

	void FixedUpdate()
	{
		jumpForce = new Vector2(0, YJumpForce);  //Creating the jumpforce applied when jumping.

		if (jump && !inJump)  //If jump is pressed and you're not already jumping.
		{

			inJump = true;

			anim.SetBool ("walking", false);

			anim.SetBool ("jump", true);

			rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);

		}

		if (inJump && hasGun) //If you're jumping and have a gun.
		{

			anim.SetBool ("gunJump", true);
		}

		if (attack && !hasGun) {     //If you press attack and dont have a gun.

			tumbleSound.Play();   //Play this sound. Sounds are set above in the start funtion.

			StartCoroutine(tumble()); //Start a coroutine that makes him tumble once. This is the "attack".

			attack = false; //Reset attack to false so he's not always set to attacking. 

		}

		if (attack && hasGun && !hasAmmo) {  //If you attack, have a gun, but have no ammo.

			gunClick.Play();

		}

		if (attack && hasGun && hasAmmo) {  //If you attack, have a gun and have ammo.

			gunShot.Play();

			ammo -= 1;   //Take one from the ammo.

			if (ammo <= 0) {  //If your ammo reaches 0.

				hasAmmo = false; //You have no ammo, so hasAmmo = false.

				gunScript.hasAmmo = false; //The gunscript also needs to know if you have ammo so it will stop working. This sets the gunScript's hasAmmo bool to false.
			}							   

		}

		if(switchWeapon && collectedGun){  //If you press the switch weapon button, you toggle between having or not having a gun.

			hasGun = !hasGun;  //Just sets it to the opposite of what it is. 
		}

		if(!hasGun) //If you don't have a gun.
		{
			anim.SetBool ("gotGun", false);  //Set the animation with a gun to false.
			gunScript.hasGun = false;   //Tell the gunscript you don't have a gun so it wont work.
			ammoCount.enabled = false;  //This hides the bullet count text so it doesn't appear next to the fist in the GUI.
		}

		if(hasGun) //If you have a gun.
		{
			anim.SetBool ("gotGun", true); //Play the right animation.
			hasGun = true; 				   // Set the bool hasGun to true, i can't remember why i had to state this here again, some criteria wasn't met without it.		
			gunScript.hasGun = true;		//Let the gun script know you have a gun so it can work again.
			ammoCount.enabled = true;		//Show the ammo count in the GUI.
		}

		if (esc) {

			Application.Quit();
		}
		

		if (left) {

			Vector3 theScale = transform.localScale;

			theScale.x = -1.3f;    //This faces him left. Not sure why i had to set it to -1.3, i'm sure its a quick fix.

			transform.localScale = theScale;



			if (!inJump) {      //If you're going left and not jumping.
				
				anim.SetBool ("walking", true);
			} 

			if (hasGun && !inJump) {  //If you're going left, have a gun and you're not jumping.
				
				anim.SetBool ("gunWalking", true);
			} 
				
			Vector3 heroPosition = moveHero.position;

			heroPosition.x += -speed;     //Move's the Hero by the speed, speed is set publicly in the inspector.

			moveHero.position = heroPosition;


		} else if (right) {              //Everything the opposite of left.

			Vector3 theScale = transform.localScale;

			theScale.x = 1.3f;

			transform.localScale = theScale;

			if (!inJump) {
				
				anim.SetBool ("walking", true);
			} 

			if (hasGun && !inJump) {
				
				anim.SetBool ("gunWalking", true);
			} 
				
			Vector3 heroPosition = moveHero.position;

			heroPosition.x += speed;

			moveHero.position = heroPosition;

		} else if (inJump && !left && !right && !jump) {  //If inJump has been set to true, you're not pressing left, right or jump.

			jumpForce = new Vector2(0, YJumpForce * -0.04f);  //Notice this is a minus force, it drags him back down from a jump if you're not pressing anything.

			rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);

		}

		else {
			anim.SetBool ("walking", false);
			anim.SetBool ("gunJump", false);      //Else set everything to false.
			anim.SetBool ("gunWalking", false);
		}

	}
		

	void OnCollisionEnter2D(Collision2D collisionObject)
	{

		if (collisionObject.gameObject.tag == "enemy" && anim.GetBool("attacking") == false) {  //If you hit an enemy and the bool "attacking" is set to false. 
			
			health -= damage;  //So, if you're no "attacking" and you hit an enemy, take damage from the health. Damage is set at the top.

			healthBar.fillAmount = (float)health;  //Set the fill amount of the health bar to whatever health is. This has to be converted to a float first, 
													// simply by writing (float) before health.
		}

		if (collisionObject.gameObject.tag == "enemy" && anim.GetBool("attacking") == true) { //If you hit an enemy and "Attacking" is set to true.

			hitSound.Play();

			enemyPos = collisionObject.gameObject.GetComponent<Transform> ();

			Vector3 enemyPosition = enemyPos.position;

			enemyPosition.x = enemyPosition.x + 50;   //Move the enemy over 50 px. This is a very very crude recycling method. Not very practical.

			enemyPos.position = enemyPosition;

		}

		if (collisionObject.gameObject.tag == "gun") {   //If you hit off the gun.

			anim.SetBool ("gotGun", true);   //Play the right animation/

			hasGun = true;  //Set hasGun to true

			collectedGun = true; //Now you can start switching weapons

			gunSound = GetComponent<AudioSource> ();    //This audio component is in the hero, it has to be moved out to the rest of the audio to keep everything tidier.

			gunSound.Play ();

			Destroy (collisionObject.gameObject);  //Destroy the object.
		}

		if (collisionObject.gameObject.tag == "keycard") {

			paper.Play();

			Destroy (collisionObject.gameObject);

			hasKey = true;    //You have the key.

		}




		if (collisionObject.gameObject.tag == "ticketMachine") { //If you hit the ticket machine

			if (hasTicket == true) {  //And have a ticket
				
				collisionObject.gameObject.GetComponent<BoxCollider2D> ().isTrigger = true; //Turn the machine's box collider to a trigger so you can pass through.
			}

		}

		anim.SetBool ("gunJump", false);

		anim.SetBool ("walking", false); //If you hit anything, set walking to false. Can't remember why this is here or if its even needed.

		anim.SetBool ("attacking", false); //If you hit anything, reset the attacking bool so you can attack again. 
	
	}

	void OnTriggerEnter2D(Collider2D other) {   //Triggers no, not Box colliders

		if (other.gameObject.tag == "subwayLeft") {  //Left subway train door

			Vector3 heroPosition = moveHero.position;

			heroPosition.x = heroPosition.x + 25; //Move the hero 25 to the right

			moveHero.position = heroPosition;

		}

		if (other.gameObject.tag == "ticket") {

			paper.Play();

			Destroy (other.gameObject);

			hasTicket = true;  //You have the subway ticket.

		}

		if (other.gameObject.tag == "subwayRight") {  //Right subway train door

			Vector3 heroPosition = moveHero.position;

			heroPosition.x = heroPosition.x - 25; //Mov the Hero 25 to the left

			moveHero.position = heroPosition;

		}

		if (other.gameObject.tag == "bandana") { 

			Destroy (other.gameObject);
			score += 10;

		}

		if (other.gameObject.tag == "ammo") {

			getAmmo.Play();

			hasAmmo = true; //You have ammo

			ammo += 10; //Add 10 to your ammo;

			Destroy (other.gameObject);

			//ammoCount.text = ammo.toString();

			gunScript.hasAmmo = true; //Let the gun scipt know that you have ammo;
		}

		if (other.gameObject.tag == "keypad" && hasKey) {

			door.SetActive (false);

			openDoor.SetActive (true);

			openingDoor.Play ();

			hasKey = false; //I have to put this here to stop the door sound from playing every time he walks past the keypad

		}
						
	}

	public void die(){
		
		Application.LoadLevel (Application.loadedLevel);  //Loads the currently loaded level.

	}

	IEnumerator tumble() //This is the coroutine called if you press "jump", or what should be called "attack" and you have no gun.
	{
		
		anim.SetBool ("jump", false); //The tumble wont work while he's jupming if the jump animation is on.

		anim.SetBool ("attacking", true);   //Set the jumping animation, or whould be called "attacking" or "tumbling" to true.

		yield return new WaitForSeconds (0.55f); //For just over a half a second

		anim.SetBool ("attacking", false); //And then turn it off again.

	}
}



