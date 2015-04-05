using System.Collections.Generic;
using TrappedGame.Main;
using TrappedGame.Model;
using TrappedGame.Model.LevelUtils;
using UnityEngine;
using UnityEngine.Cloud.Analytics;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
    public class LevelUIController : MonoBehaviour {

        public TutorialMenu tutorialMenu;
        public WinMenu winMenu;
        public Text levelName;

        private Game game;
        public Game Game {
            set {
                game = value;
                levelName.text = game.Level.Name;
            }
        }

        public void ShowTutorial(Tutorial tutorial) {
            tutorialMenu.ShowTutorial(tutorial);
        }

        public void ShowWinMenu() {
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

        public void RateLevel(bool like) {
            var levelName = PlayerPrefs.GetString(Preferences.CURRENT_LEVEL);
            UnityAnalytics.CustomEvent("RateLevel", new Dictionary<string, object> {
                { "levelName", levelName },
                { "isLike", like }
            });
        }

        public void RateLevelDifficulty(string difficulty) {
            var levelName = PlayerPrefs.GetString(Preferences.CURRENT_LEVEL);
            UnityAnalytics.CustomEvent("RateLevelDifficulty", new Dictionary<string, object> {
                { "levelName", levelName },
                { "difficulty", difficulty }
            });
        }
	}
}