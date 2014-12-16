using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using TrappedGame.Model.LevelLoader;
using TrappedGame.Model.LevelUtils;
using TrappedGame.Model.Common;

namespace TrappedGame.Model.LevelLoader.Json {
	public class JsonLevelLoader : ILevelLoader {

        // TODO Remove fields (not thread save)
        private JSONNode level;
        private IDictionary<char, JSONNode> descriptions;
        private LevelBuilder builder;

		public Level LoadLevel(string fileName) {
			var levelFile = Resources.Load<TextAsset>(fileName);
			level = JSON.Parse(levelFile.text);

			descriptions = new Dictionary<char, JSONNode>();

			ReadLevelCommonInfo();
			ReadLevelDescription();
			ReadLevelMap();
            
            return builder.Build();
		}

		private void ReadLevelCommonInfo() {
			var levelName = level["name"].Value;
			var levelXSize   = level["size"]["x"].AsInt;
			var levelYSize   = level["size"]["y"].AsInt;

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
			var defaultCellBuilder = new DefaultCellBuilder();
			var cellBuilder = new CellBuilder();	

			var rowCount = level["map"].AsArray.Count;

			for(int y = 0; y < rowCount; y++) {
				string row = level["map"].AsArray[y];

				for(int x = 0; x < row.Length; x++) {
					var element = row[x];

					if(descriptions.ContainsKey(element)) {
						cellBuilder.MakeCell(descriptions[element], builder, new IntVector2(x, y));
					} else {
						JSONNode nodeElement = new JSONClass();
						nodeElement["element"] = element.ToString();
						defaultCellBuilder.MakeCell(nodeElement, builder, new IntVector2(x, y));
					}
				}
			}
		}
	}
}