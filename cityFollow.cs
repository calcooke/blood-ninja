using UnityEngine;
using System.Collections;

public class cityFollow : MonoBehaviour {

	public GameObject hero;

	void Start () {
	
	}

	void Update () {

		transform.position = new Vector3 (hero.transform.position.x + 2.2f, hero.transform.position.y - 1.5f, transform.position.z);
	
	}					//Get the Transform of the skyline, and set it's position to be the same as the hero object, with a bit of moving into position,
}						// but keep it's own Z index.
