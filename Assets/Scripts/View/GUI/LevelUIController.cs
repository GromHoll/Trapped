using TrappedGame.Main;
using TrappedGame.Model;
using TrappedGame.Model.LevelUtils;
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

        public void NextLevel() {
            var levelName = PlayerPrefs.GetString(Preferences.CURRENT_LEVEL);
            var nextLevel = Levels.GetNext(levelName);
            if (nextLevel != null) {
                PlayerPrefs.SetString(Preferences.CURRENT_LEVEL, nextLevel.Path);
                ReloadLevel();
            } else {
                BackToMainMenu();
            }
        }

        private void LoadScene(string sceneName) {
            Application.LoadLevel(sceneName);
        }
	}
}