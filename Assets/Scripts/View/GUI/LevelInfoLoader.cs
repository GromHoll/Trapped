using UnityEngine;
using System.Collections.Generic;

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
		}

		public ICollection<string> GetPackNames() {
			return levelsInfo.Keys;
		}

		public List<LevelT> GetLevelPaths(string packName) {
			if(levelsInfo.ContainsKey(packName)) {
				return levelsInfo[packName];
			}
			return null;
		}

		private void LoadLevels() {
            var levelList = Resources.Load<TextAsset>("Levels/Levels");
			var json = JSON.Parse(levelList.text);

			string packName;
			string packFolder;
			string levelName;

			foreach (JSONNode pack in json["Packs"].AsArray) {
				packName   = pack["Name"].Value;
				packFolder = pack["Folder"].Value;

				if(!levelsInfo.ContainsKey(packName)) {
					levelsInfo[packName] = new List<LevelT>();
				}

				foreach(JSONNode level in pack["Levels"].AsArray) {
					levelName = (level["Name"].Value.Length!=0 ? level["Name"].Value : level["Source"].Value);
					levelsInfo[packName].Add(new LevelT(levelName, packFolder + level["Source"].Value));
				}
			}
		}
	}
}
