using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	private Transform enemyPos;

	public float speed; //Speed at which the enemies move, set publicly.

	public float distance; //distance they move, set publicly.

	public bool walk; //A bool needed just to toggle bewteen true and false, making them go right or left.

    

	void Start () {

		StartCoroutine(move()); //starts the coroutine to toggle the walk bool.

	}

	void Update () {


        
		if (walk == true) {   //If walk is true, moves them right.
			
			enemyPos = GetComponent<Transform> ();
			Vector3 movePos = enemyPos.position;
			movePos.x += speed;    					//Change their x position by the speed.
			enemyPos.position = movePos;
			Vector3 theScale = transform.localScale;
			theScale.x = +1.3f;
			transform.localScale = theScale;
		} else {									//If walk isnt true, do the opposite.
			Vector3 movePos = enemyPos.position;
			movePos.x -= speed;
			enemyPos.position = movePos;
			Vector3 theScale = transform.localScale;
			theScale.x = -1.3f;
			transform.localScale = theScale;
		}


	}
		
	IEnumerator move()
	{
		while (true) {  //While true is true, which is forever.
			
			walk = !walk;                                  //Walk is equal to the opposite of whatever it is.
			yield return new WaitForSeconds (distance*Random.Range(0.3f, 1.2f));	   // Wait for the distance before happening again.	
		}

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		                           
		if (other.gameObject.tag == "safetyNet") {     //If the eney hits one of the "safety net" triggers, to stop them from being trapped between the buildings or
														// falling through the ground between the subways
			Vector3 enemyPosition = enemyPos.position;

			enemyPosition.x = enemyPosition.x + 50;   //Move the enemy over 50 px. Crude method.

			enemyPos.position = enemyPosition;

		}
	}
}

