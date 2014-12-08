using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using SimpleJSON;

namespace TrappedGame.View.GUI {
	public class LevelInfoLoader  {
		private IDictionary<string, ArrayList> levelsInfo;

		public LevelInfoLoader() {
			levelsInfo = new Dictionary<string, ArrayList>();
			LoadLevels();
		}

		public ICollection<string> GetPackNames() {
			return levelsInfo.Keys;
		}

		public ArrayList GetLevelPaths(string packName) {
			if(levelsInfo.ContainsKey(packName)) {
				return levelsInfo[packName];
			} else {
				//TODO Pack not exist EXCEPTION
			}

			return null;
		}

		private void LoadLevels() {
			var levelList = Resources.Load<TextAsset>("Levels");
			var json = JSON.Parse (levelList.text);

			string packName;
			foreach (JSONNode pack in json["Packs"].AsArray) {
				packName = pack["PackName"].Value;

				if(!levelsInfo.ContainsKey(packName)) {
					levelsInfo[packName] = new ArrayList();
				} else {
					//TODO Packs with same name EXCEPTION
				}

				foreach(JSONNode levelName in pack["Levels"].AsArray) {
					levelsInfo[packName].Add(levelName.Value);
				}
			}
		}

		private void Print() {
			foreach(string packName in levelsInfo.Keys) {
				foreach(string levelName in levelsInfo[packName]){
					Debug.Log(packName + " : " + levelName);
				}
			}
		}

	}
}
