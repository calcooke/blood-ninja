using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement; //You need to use this for switching between scenes
using UnityEngine.UI;

public class sceneManager : MonoBehaviour {

	public GameObject musicPlayer; 

	void Awake(){

		musicPlayer = GameObject.Find ("MusicPlay");  //Find the music

		DontDestroyOnLoad (musicPlayer);  //Dont destroy it when the next scene loads

	}

	public void changeScene(){						//Function called from the play button, changes the scene

		SceneManager.LoadScene ("Blood_Ninja");   
	}

	public void loadMenu(){					 //Function that loads the Menu again once you've won, from the "Menu" button

		SceneManager.LoadScene ("Menu");  

		musicPlayer.SetActive (false);  //Turns off the music so it can start again, it will just be playing twice otherwise.
	}

	public void toggleActive(){   //function called from the controls button, toggles the controls panel on and off from the Menu. 
									
		bool currentActiveState = gameObject.activeSelf;   //You set its current active state to a boolean

		gameObject.SetActive (!currentActiveState); 		//And set it to be the opposite of what it is

	}

	public void quitGame(){   //This quits the Game when you press the Exit button in the menu

		Application.Quit();

	}
}
