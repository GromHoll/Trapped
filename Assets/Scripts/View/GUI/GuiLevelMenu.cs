using UnityEngine;
using System.Collections;

public class GuiLevelMenu : MonoBehaviour {

	void OnGUI() {
		GUI.Box (new Rect(10, 10, 100, 150), "Levels");

		if (GUI.Button(new Rect(20, 40, 80, 20), "Level 1")) {
			PlayerPrefs.SetString("CurrentLevel", "Levels/Level1");
			Application.LoadLevel("Level");
		}

		if (GUI.Button(new Rect(20, 70, 80, 20), "Level 2")) {
			PlayerPrefs.SetString("CurrentLevel", "Levels/Level2");
			Application.LoadLevel("Level");
		}

		if (GUI.Button(new Rect(20, 100, 80, 20), "Level 3")) {
			PlayerPrefs.SetString("CurrentLevel", "Levels/Level3");
			Application.LoadLevel("Level");
		}

		if (GUI.Button(new Rect(20, 130, 80, 20), "Level 4")) {
			PlayerPrefs.SetString("CurrentLevel", "Levels/Level4");
			Application.LoadLevel("Level");
		}
	}

}
