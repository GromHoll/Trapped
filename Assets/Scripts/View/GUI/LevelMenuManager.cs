using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using SimpleJSON;
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
			//TODO Destroy Children
			Packs.transform.DetachChildren ();

			foreach (string packName in levelInfo.GetPackNames()) {
				CreatePackButton(packName);
			}

			Levels.SetActive (false);
			Packs.SetActive (true);
		}

		public void OpenLevels(string packName) {
			//TODO Destroy Children
			Levels.transform.DetachChildren();

			foreach (string levelName in levelInfo.GetLevelPaths(packName)) {
				CreateLevelButton(levelName);
			}

			Levels.SetActive(true);
			Packs.SetActive(false);
		}

		private void CreatePackButton(string packName) {
			var button = GameUtils.InstantiateChildForWorld(buttonPrefab, Vector2.zero, Packs, false);
			button.GetComponent<Button>().onClick.AddListener(() => {
				OpenLevels(packName);
			});
			button.transform.GetChild(0).GetComponent<Text>().text = packName;
		}

		private void CreateLevelButton(string levelName) {
			var button = GameUtils.InstantiateChildForWorld(buttonPrefab, Vector2.zero, Levels, false);
			button.GetComponent<Button>().onClick.AddListener(() => {
				PlayerPrefs.SetString(Preferences.CURRENT_LEVEL, levelName);
				Application.LoadLevel("Level");
			});
		}

		public void Back() {
			if (Levels.activeSelf) {
				Levels.SetActive (false);
				Packs.SetActive (false);
				OpenPacks ();
			} else {
				Levels.SetActive (false);
				Packs.SetActive (false);
				MainMenu.SetActive(true);
			}
		}

		public void Quit() {
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		}

		private void LoadLevelsInfo() {

		}
	}
}
