using System;
using System.Linq;
using TrappedGame.Utils;
using UnityEngine;
using System.Collections.Generic;
using SimpleJSON;
using TrappedGame.Model.LevelUtils;
using TrappedGame.Model.Common;

namespace TrappedGame.Model.LevelLoader.Json {
	public class JsonLevelLoader : ILevelLoader {

	    private const string LEVEL_NAME_KEY = "name";
        private const string LEVEL_SIZE_KEY = "size";
        private const string TUTORIAL_KEY = "tutorial";
        private const string TUTORIAL_NAME_KEY = "name";
        private const string TUTORIAL_MESSAGE_KEY = "message";
        private const string SYMBOLS_KEY = "symbols";
        private const string SYMBOL_KEY = "symbol";
        private const string MAP_KEY = "map";

        private const int LEVEL_SIZE_X_INDEX = 0;
        private const int LEVEL_SIZE_Y_INDEX = 1;

        private readonly JsonCellBuiler cellBuilder = new JsonCellBuiler();
        
		public Level LoadLevel(string fileName) {
            Debug.Log("Load level = " + fileName);
			var levelFile = Resources.Load<TextAsset>(fileName);
            var jsonLevel = JSON.Parse(levelFile.text);
            var builder = ReadLevel(jsonLevel);
            return builder.Build();
		}

        private LevelBuilder CreateBuilerForLevel(JSONNode jsonLevel) {
            var levelName  = jsonLevel[LEVEL_NAME_KEY].Value;
            var levelXSize = jsonLevel[LEVEL_SIZE_KEY][LEVEL_SIZE_X_INDEX].AsInt;
            var levelYSize = jsonLevel[LEVEL_SIZE_KEY][LEVEL_SIZE_Y_INDEX].AsInt;
			return new LevelBuilder(levelName, levelXSize, levelYSize);
		}

        private IDictionary<char, JSONNode> ReadLevelCodeSymbols(JSONNode jsonLevel) {
            var symbols = new Dictionary<char, JSONNode>();
            foreach (JSONNode symbolNode in jsonLevel[SYMBOLS_KEY].AsArray) {
                var symbol = symbolNode[SYMBOL_KEY].Value;
                Validate.CheckArgument(symbol.Any(), "Symbol info is empty");
                Validate.CheckArgument(symbol.Count() == 1, "Symbol info is too long");
                symbols[symbol[0]] = symbolNode;
			}
            return symbols;
        }

        private LevelBuilder ReadLevel(JSONNode jsonLevel) {
            var builder = CreateBuilerForLevel(jsonLevel);
            var symbols = ReadLevelCodeSymbols(jsonLevel);

            var rowCount = builder.GetSize().y;
            var rowLenght = builder.GetSize().x;

			for (var y = 0; y < rowCount; y++) {
                string row = jsonLevel[MAP_KEY][y];
                for (var x = 0; x < rowLenght; x++) {
					var element = row[x];
                    Validate.CheckArgument(symbols.ContainsKey(element), String.Format("Cannot find element '{0}' in symbols", element));
                    cellBuilder.MakeCell(symbols[element], builder, new IntVector2(x, rowCount - y - 1));
				}
			}

            var tutorial = jsonLevel[TUTORIAL_KEY];
            builder.TutorialName = tutorial[TUTORIAL_KEY][TUTORIAL_NAME_KEY];
            builder.TutorialMessage = tutorial[TUTORIAL_KEY][TUTORIAL_MESSAGE_KEY];

            return builder;
        }
	}
}