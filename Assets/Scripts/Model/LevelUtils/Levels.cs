using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using SimpleJSON;
using TrappedGame.Utils;
using UnityEngine;

namespace TrappedGame.Model.LevelUtils {
    // TODO How about storing current level in Levels (not in Preferences)?
	public class Levels {

	    public class LevelInfo {
            public PackInfo Pack { get; internal set; }
            public string Source { get; private set; }
            public string Name { get; private set; }
	        public string Path {
	            get { return Pack.Folder + Source; }
	        }

	        internal LevelInfo(string source, string name = null) {
                Validate.NotNullOrEmpty(source, "Source can't be a null or empty");
                Source = source;
                Name = name ?? source;
            }
	    }

        public class PackInfo {
            public string Name { get; private set; }
            public string Folder { get; private set; }
            public IList<LevelInfo> Levels { get; private set; }

            internal PackInfo(string name, string folder) {
                Validate.NotNullOrEmpty(name, "Name can't be a null or empty");
                Validate.NotNullOrEmpty(folder, "Folder can't be a null or empty");
                Name = name;
                Folder = folder;
                Levels = new List<LevelInfo>();
            }

            internal void AddLevelInfo(LevelInfo level) {
                level.Pack = this;
                Levels.Add(level);
            }
	    }

        public static readonly string LEVELS_FILENAME = "Levels/Levels";

	    private const String PACKS_KEY = "Packs";
        private const String PACK_NAME_KEY = "Name";
        private const String PACK_FOLDER_KEY = "Folder";
        private const String LEVELS_KEY = "Levels";
        private const String LEVEL_NAME_KEY = "Name";
        private const String LEVEL_SOURCE_KEY = "Source";


        private static readonly Levels Instance = new Levels();

	    private IList<PackInfo> packs = new List<PackInfo>();

	    private Levels() {
            var levelsText = Resources.Load<TextAsset>(LEVELS_FILENAME);
            var jsonLevels = JSON.Parse(levelsText.text);

            foreach (JSONNode packNode in jsonLevels[PACKS_KEY].AsArray) {
                var packName = packNode[PACK_NAME_KEY].Value;
                var packFolder = packNode[PACK_FOLDER_KEY].Value;
                var pack = new PackInfo(packName, packFolder);

                foreach (JSONNode levelNode in packNode[LEVELS_KEY].AsArray) {
                    var levelName = levelNode[LEVEL_NAME_KEY];
                    var levelSource = levelNode[LEVEL_SOURCE_KEY];
                    var level = new LevelInfo(levelSource, levelName);
                    pack.AddLevelInfo(level);
                }
                packs.Add(pack);
            }
	    }

        public static IList<PackInfo> GetPacks() {
            return Instance.packs;
        }

        public static LevelInfo GetNext(string currentName) {
            var currentLevelFound = false;
            foreach (var pack in Instance.packs) {
                foreach (var level in pack.Levels) {
                    if (currentLevelFound) {
                        return level;
                    } else if (level.Path == currentName) {
                        currentLevelFound = true;
                    }
                }
            }
            return null;
        }
	}
}
