using TrappedGame.Main;
using TrappedGame.Model.LevelUtils;
using TrappedGame.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
	public class LevelsMenu : MonoBehaviour {

	    public Button backButton;
	    public Button buttonPrefab;
        public GameObject levelsPanel;

        public Levels.PackInfo PackInfo { get; set; }
        public PacksMenu PacksMenu { get; set; }

	    void Start() {
	        backButton.onClick.AddListener(() => {
                PacksMenu.gameObject.SetActive(true);
                gameObject.SetActive(false);
	        });

            var levels = PackInfo.Levels;
            foreach (var levelInfo in levels) {
                CreateLevelButton(levelInfo);
            }
	    }

        private void CreateLevelButton(Levels.LevelInfo level) {
            var buttonGO = GameObjectUtils.InstantiateChildForWorld(buttonPrefab.gameObject, Vector2.zero,
                                                                    levelsPanel.gameObject, false);
            var button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() => {
                PlayerPrefs.SetString(Preferences.CURRENT_LEVEL, level.Path);
                // TODO Change Level string to constant
                var mainMenu = GameObject.Find("MainMenuUI");
                if (mainMenu != null) {
                    mainMenu.SetActive(false);
                }
                Application.LoadLevel("Level");
            });
            button.GetComponentInChildren<Text>().text = level.Name;
        }

	}
}
