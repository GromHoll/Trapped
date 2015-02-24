using TrappedGame.Main;
using TrappedGame.Model.LevelUtils;
using TrappedGame.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
	public class LevelsMenu : MonoBehaviour {

	    public Button backButton;
	    public Button buttonPrefab;

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
            var buttonGO = GameObjectUtils.InstantiateChild(buttonPrefab.gameObject, Vector2.zero, gameObject);
            var button = buttonGO.GetComponent<Button>();
            button.onClick.AddListener(() => {
                PlayerPrefs.SetString(Preferences.CURRENT_LEVEL, level.Path);
                // TODO Channge Level string to constant
                Application.LoadLevel("Level");
            });
            button.GetComponentInChildren<Text>().text = level.Name;
        }

	}
}
