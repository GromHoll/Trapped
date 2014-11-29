using TrappedGame.Main;
using TrappedGame.Utils;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;
using SimpleJSON;

namespace TrappedGame {
	public class LevelMenuLoader : MonoBehaviour {

		public GameObject buttonPrefab;

		void Start() {
			TextAsset levelList = Resources.Load<TextAsset>("LevelList");
			var json = JSON.Parse (levelList.text);

			foreach (JSONNode levelName in json["Levels"].AsArray) {
				CreateChildButton(levelName.Value);
			}
			GridLayoutGroup layout = GetComponent<GridLayoutGroup>();
			layout.CalculateLayoutInputHorizontal();
			//LayoutRebuilder.MarkLayoutForRebuild(layout.);
		}

		private void CreateChildButton(string levelName) {
			GameObject button = GameUtils.InstantiateChild(buttonPrefab, Vector2.zero, gameObject);
			button.GetComponent<Button>().onClick.AddListener(() => {
				PlayerPrefs.SetString(Preferences.CURRENT_LEVEL, levelName);
				Application.LoadLevel("Level");
			}); 
			button.transform.SetParent(gameObject.transform, false);
		}
	}
}