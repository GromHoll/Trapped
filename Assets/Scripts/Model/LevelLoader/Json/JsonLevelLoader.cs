using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using TrappedGame.Model.LevelLoader;
using TrappedGame.Model.LevelUtils;
using TrappedGame.Model.Common;

namespace TrappedGame.Model.LevelLoader.Json {
	public class JsonLevelLoader : ILevelLoader {
		public Level LoadLevel(string fileName) {
			TextAsset levelFile = Resources.Load<TextAsset>(fileName);
			level = JSON.Parse(levelFile.text);

			descriptions = new Dictionary<char, JSONNode>();

			ReadLevelCommonInfo();
			ReadLevelDescription();
			ReadLevelMap();

			return new Level(builder);
		}

		private void ReadLevelCommonInfo() {
			string levelName = level["name"].Value;
			int levelXSize   = level["size"]["x"].AsInt;
			int levelYSize   = level["size"]["y"].AsInt;

			builder = new LevelBuilder(levelName, levelXSize, levelYSize);
		}

		private void ReadLevelDescription() {
			foreach (JSONNode description in level["description"].AsArray) {
				descriptions.Add(
					new KeyValuePair<char, JSONNode>(
						description["element"].Value.ToCharArray()[0],
						description
					)
				);
			}
		}

		private void ReadLevelMap() {
			DefaultCellBuilder defaultCellBuilder = new DefaultCellBuilder();
			CellBuilder cellBuilder = new CellBuilder();	

			int rowCount = level["map"].AsArray.Count;

			for(int y = 0; y < rowCount; y++) {
				string row = level["map"].AsArray[y];

				for(int x = 0; x < row.Length; x++) {
					char element = row[x];

					if(descriptions.ContainsKey(element)) {
						cellBuilder.MakeCell(descriptions[element], builder, new IntVector2(x, y));
					} else {
						defaultCellBuilder.MakeCell(descriptions[element], builder, new IntVector2(x, y));
					}
				}
			}
		}

		private JSONNode level;
		private IDictionary<char, JSONNode> descriptions;
		private LevelBuilder builder;
	}
}