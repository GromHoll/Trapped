using UnityEngine;
using System.Collections;

public class WinMenu : MonoBehaviour {

	void OnGUI() {
		GUI.Box (new Rect(10, 10, 100, 150), "WIN!\n Score = " + PlayerPrefs.GetInt("Score")
		         + "\nYou die " + PlayerPrefs.GetInt("Death") + " count");
		
		if (GUI.Button(new Rect(20, 80, 80, 20), "Back!")) {
			Application.LoadLevel("MainMenu");
		}
	}
}
