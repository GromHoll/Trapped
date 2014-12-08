using UnityEngine;
using System.Collections.Generic;
using System.Collections;

using SimpleJSON;

namespace TrappedGame.View.GUI {
	public class LevelT {
		public LevelT(string _name, string _path) {
			name = _name;
			path = _path;
		}

		public string name;
		public string path;
	}

	public class LevelInfoLoader  {
		private IDictionary<string, List<LevelT>> levelsInfo;

		public LevelInfoLoader() {
			levelsInfo = new Dictionary<string, List<LevelT>>();
			LoadLevels();
			Print ();
		}

		public ICollection<string> GetPackNames() {
			return levelsInfo.Keys;
		}

		public List<LevelT> GetLevelPaths(string packName) {
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
			string packFolder;

			foreach (JSONNode pack in json["Packs"].AsArray) {
				packName   = pack["Name"].Value;
				packFolder = pack["Folder"].Value;

				if(!levelsInfo.ContainsKey(packName)) {
					levelsInfo[packName] = new List<LevelT>();
				} else {
					//TODO Packs with same name EXCEPTION
				}

				foreach(JSONNode level in pack["Levels"].AsArray) {
					levelsInfo[packName].Add(new LevelT(level["Name"].Value, packFolder + level["Source"].Value));
				}
			}
		}

		private void Print() {
			foreach(string packName in levelsInfo.Keys) {
				foreach(LevelT level in levelsInfo[packName]){
					Debug.Log(packName + " : " + level.name);
				}
			}
		}

	}
}
