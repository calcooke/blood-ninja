using UnityEngine;
using System.Collections;

public class bulletScript : MonoBehaviour {

	private Renderer renderer;

	private Transform enemyPos; //This is the transform of the enemy, for moving it elsewhere when hit.

	private bool seen;

	void Start () {

		renderer = GetComponent<Renderer> ();   //Get the rendereder of the bullet

		seen = false;  //We can't see it.
	
	}

	void Update () {

		if (renderer.isVisible) {  //If a ullet has rendered
			seen = true;          // We can see it
		}

		if ((renderer.isVisible == false) && (seen == true)) {     //If you can't see the  bullet anymore, but it has been seen - as in if it moves off screen.
			Destroy (gameObject);									//Destroy it.
		}
	
	}


	void OnCollisionEnter2D(Collision2D collisionObject)
	{

		Destroy (gameObject);                               //Destroy the bullet if it hits something.

		if (collisionObject.gameObject.tag == "enemy") {     //Destroy an enemy if it hits it. This needs to be worked in a way that recycles the enemy.


			enemyPos = collisionObject.gameObject.GetComponent<Transform>();

			Vector3 enemyPosition = enemyPos.position;

			enemyPosition.x = enemyPosition.x + 50;   //Move the enemy over 50 px. This is a very very crude recycling method. Not very practical.

			enemyPos.position = enemyPosition;


		}

	}

}
