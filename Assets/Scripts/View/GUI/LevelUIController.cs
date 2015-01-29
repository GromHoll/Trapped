using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
    public class LevelUIController : MonoBehaviour {


        public void BackToMainMenu() {
            LoadScene("MainMenu");
        }

        public void ReloadLevel() {
            // TODO I think reload full scene os bad idea
            LoadScene("Level");
        }

        private void LoadScene(string sceneName) {
			Application.LoadLevel(sceneName);	                       
		}
	}
}