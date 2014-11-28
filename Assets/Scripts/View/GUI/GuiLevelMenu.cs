using UnityEngine;
using System.Collections;

public class GuiLevelMenu : MonoBehaviour {

	public void LoadLevel(string levelName) {
		PlayerPrefs.SetString("CurrentLevel", levelName);
		Application.LoadLevel("Level");
	}
}
