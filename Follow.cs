using UnityEngine;
using System.Collections;

public class Follow : MonoBehaviour {

	public GameObject player;


	void LateUpdate() 
	{
		float newXPosition = player.transform.position.x;
		float newYPosition = player.transform.position.y;

		transform.position = new Vector3(newXPosition, newYPosition + 2, transform.position.z);  //I push the Y position up 2 because we could see too much of the ground.
	}																							//Remove or change and you'll see what i mean.
}
