using UnityEngine;

namespace TrappedGame {
    public class GuiLevelMenu : MonoBehaviour {

    	public void LoadLevel(string levelName) {
            PlayerPrefs.SetString(Preferences.CURRENT_LEVEL, levelName);
    		Application.LoadLevel("Level");
    	}
    }
}
