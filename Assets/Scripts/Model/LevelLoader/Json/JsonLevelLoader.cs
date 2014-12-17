using System.Runtime.Remoting.Messaging;
using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using TrappedGame.Model.LevelUtils;
using TrappedGame.Model.Common;

namespace TrappedGame.Model.LevelLoader.Json {
	public class JsonLevelLoader : ILevelLoader {
        
		public Level LoadLevel(string fileName) {
			var levelFile = Resources.Load<TextAsset>(fileName);
            var jsonLevel = JSON.Parse(levelFile.text);
            var builder = ReadLevelCommonInfo(jsonLevel);
            var descriptions = ReadLevelDescription(jsonLevel);
            ReadLevelMap(jsonLevel, descriptions, builder);
            
            return builder.Build();
		}

        private LevelBuilder ReadLevelCommonInfo(JSONNode jsonLevel) {
            var levelName = jsonLevel["name"].Value;
            var levelXSize = jsonLevel["size"]["x"].AsInt;
            var levelYSize = jsonLevel["size"]["y"].AsInt;
			return new LevelBuilder(levelName, levelXSize, levelYSize);
		}

        private IDictionary<char, JSONNode> ReadLevelDescription(JSONNode jsonLevel) {
            IDictionary<char, JSONNode> descriptions = new Dictionary<char, JSONNode>();
            foreach (JSONNode description in jsonLevel["description"].AsArray) {
				descriptions.Add(
					new KeyValuePair<char, JSONNode>(
						description["element"].Value.ToCharArray()[0],
						description
					)
				);
			}
            return descriptions;
        }

        private void ReadLevelMap(JSONNode jsonLevel, IDictionary<char, JSONNode> descriptions, LevelBuilder builder) {
			var defaultCellBuilder = new DefaultCellBuilder();
			var cellBuilder = new CellBuilder();

            var rowCount = jsonLevel["map"].AsArray.Count;

			for(int y = 0; y < rowCount; y++) {
                string row = jsonLevel["map"].AsArray[y];

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