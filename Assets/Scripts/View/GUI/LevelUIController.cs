using TrappedGame.Model;
using UnityEngine;

namespace TrappedGame.View.GUI {
    public class LevelUIController : MonoBehaviour {

        public TutorialMenu tutorialMenu;
        public WinMenu winMenu;

        public void ShowTutorial(Tutorial tutorial) {
            tutorialMenu.ShowTutorial(tutorial);
        }

        public void ShowWinMenu(Game game) {
            winMenu.Show(game);
        }

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