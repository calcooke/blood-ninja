using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameController : MonoBehaviour {      //This script does nothing, i was gonna see how much of the code i could move in here.

	private static GameController instance;


	public static GameController getInstance()
	{
		return instance;
	}

	void Start(){

		instance = this;
	}
		

}