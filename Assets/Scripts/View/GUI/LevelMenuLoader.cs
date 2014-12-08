using SimpleJSON;
using TrappedGame.Main;
using TrappedGame.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
	public class LevelMenuLoader : MonoBehaviour {

		public GameObject buttonPrefab;

		void Start() {


			var levelList = Resources.Load<TextAsset>("LevelList");
			var json = JSON.Parse (levelList.text);

			foreach (JSONNode levelName in json["Levels"].AsArray) {
				CreateChildButton(levelName.Value);
			}
		}

		private void CreateChildButton(string levelName) {
			var button = GameUtils.InstantiateChild(buttonPrefab, Vector2.zero, gameObject);
			button.GetComponent<Button>().onClick.AddListener(() => {
				PlayerPrefs.SetString(Preferences.CURRENT_LEVEL, levelName);
				Application.LoadLevel("Level");
			});

			//TODO Add level Name
			//button.GetComponent<Text>().text = levelName;

			button.transform.SetParent(gameObject.transform, false);
		}
	}
}