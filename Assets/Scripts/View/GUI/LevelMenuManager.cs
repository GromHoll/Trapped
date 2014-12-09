using UnityEngine;
using TrappedGame.Main;
using TrappedGame.Utils;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
	public class LevelMenuManager : MonoBehaviour {

		public GameObject MainMenu;
		public GameObject Levels;
		public GameObject Packs;

		public GameObject buttonPrefab;

		LevelInfoLoader levelInfo;

		void Start() {
			levelInfo = new LevelInfoLoader();

			Levels.SetActive(false);
			Packs.SetActive(false);
		}

		public void OpenPacks() {
			GameUtils.DestroyAllChild(Packs);

			foreach (string packName in levelInfo.GetPackNames()) {
				CreatePackButton(packName);
			}

			Show(Packs);
		}

		public void OpenLevels(string packName) {
			GameUtils.DestroyAllChild(Levels);

			foreach (LevelT level in levelInfo.GetLevelPaths(packName)) {
				CreateLevelButton(level);
			}

			Show(Levels);
		}

		public void Back() {
			if (Levels.activeSelf) {
				OpenPacks ();
			} else if (Packs.activeSelf) {
				Show (MainMenu);
			} else {
				Quit();
			}
		}

		public void Quit() {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		}

		private void CreatePackButton(string packName) {
			var button = GameUtils.InstantiateChildForWorld(buttonPrefab, Vector2.zero, Packs, false);
			button.GetComponent<Button>().onClick.AddListener(() => {
				OpenLevels(packName);
			});
			button.transform.GetChild(0).GetComponent<Text>().text = packName;
		}
		
		private void CreateLevelButton(LevelT level) {
			var button = GameUtils.InstantiateChildForWorld(buttonPrefab, Vector2.zero, Levels, false);
			button.GetComponent<Button>().onClick.AddListener(() => {
				PlayerPrefs.SetString(Preferences.CURRENT_LEVEL, level.path);
				Application.LoadLevel("Level");
			});
			button.transform.GetChild(0).GetComponent<Text>().text = level.name;
		}

		private void Show(GameObject toShow) {
			Levels.SetActive (false);
			Packs.SetActive (false);
			MainMenu.SetActive(false);

			toShow.SetActive(true);
		}
	}
}
