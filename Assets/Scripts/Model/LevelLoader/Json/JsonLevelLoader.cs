using UnityEngine;
using SimpleJSON;
using TrappedGame.Model.LevelLoader;
using TrappedGame.Model.LevelUtils;

namespace TrappedGame.Model.LevelLoader.Json {
	public class JsonLevelLoader : ILevelLoader {
		public Level LoadLevel(string fileName) {
			TextAsset levelFile = Resources.Load<TextAsset>(fileName);
			level = JSON.Parse(levelFile.text);

			ReadLevelCommonInfo();
			ReadLevelMap();

			return new Level(builder);
		}

		private void ReadLevelCommonInfo() {
			string levelName = level["name"].Value;
			int levelXSize   = level["size"]["x"].AsInt;
			int levelYSize   = level["size"]["y"].AsInt;

			builder = new LevelBuilder(levelName, levelXSize, levelYSize);
		}

		private void ReadLevelMap() {

		}

		private JSONNode level;
		private LevelBuilder builder;
	}
}