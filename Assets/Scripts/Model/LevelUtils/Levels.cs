using SimpleJSON;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using TrappedGame.Utils;
using UnityEngine;
using FilePath = System.IO.Path;

namespace TrappedGame.Model.LevelUtils {
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
            public bool External { get; private set; }
            public IList<LevelInfo> Levels { get; private set; }

            internal PackInfo(string name, string folder, bool external) {
                Validate.NotNullOrEmpty(name, "Name can't be a null or empty");
                Validate.NotNullOrEmpty(folder, "Folder can't be a null or empty");
                Name = name;
                Folder = folder;
                External = external;
                Levels = new List<LevelInfo>();
            }

            internal void AddLevelInfo(LevelInfo level) {
                level.Pack = this;
                Levels.Add(level);
            }

            internal void AddLevelInfo(string source, string name = null) {
                var levelInfo = new LevelInfo(source, name);
                AddLevelInfo(levelInfo);
            }
	    }

        public static readonly string LEVELS_FILENAME = "Levels/Levels";

	    private const String PACKS_KEY = "Packs";
        private const String PACK_NAME_KEY = "Name";
        private const String PACK_FOLDER_KEY = "Folder";
        private const String PACK_LOAD_ALL_KEY = "LoadAll";
        private const String PACK_EXTERNAL_KEY = "External";
        private const String LEVELS_KEY = "Levels";
        private const String LEVEL_NAME_KEY = "Name";
        private const String LEVEL_SOURCE_KEY = "Source";


        private static readonly Levels Instance = new Levels();

	    private IList<PackInfo> packs = new List<PackInfo>();

        // TODO Separate Levels logic and Levels Loading
	    private Levels() {
            var levelsText = Resources.Load<TextAsset>(LEVELS_FILENAME);
            var jsonLevels = JSON.Parse(levelsText.text);

            foreach (JSONNode packNode in jsonLevels[PACKS_KEY].AsArray) {
                var packName = packNode[PACK_NAME_KEY].Value;
                var packFolder = packNode[PACK_FOLDER_KEY].Value;
                var external = packNode[PACK_EXTERNAL_KEY].AsBool;
                var loadAll = packNode[PACK_LOAD_ALL_KEY].AsBool;
                var pack = new PackInfo(packName, packFolder, external);
                var loadedPack = loadAll ? LoadAllFolder(pack) : LoadList(pack, packNode);
                packs.Add(loadedPack);
            }
	    }

        private PackInfo LoadList(PackInfo pack, JSONNode packNode) {
            foreach (JSONNode levelNode in packNode[LEVELS_KEY].AsArray) {
                var levelName = levelNode[LEVEL_NAME_KEY];
                var levelSource = levelNode[LEVEL_SOURCE_KEY];
                pack.AddLevelInfo(levelSource, levelName);
            }
            return pack;
        }

        private PackInfo LoadAllFolder(PackInfo pack) {
            return pack.External ? LoadAllExternalFolder(pack) : LoadAllResourcesFolder(pack);
        }

        private PackInfo LoadAllExternalFolder(PackInfo pack) {
            var files = GetAllFilesInFolder(pack.Folder);
            foreach (var filePath in files) {
                var fileName = FilePath.GetFileName(filePath);
                pack.AddLevelInfo(fileName);
            }
            return pack;
        }

        private IEnumerable<String> GetAllFilesInFolder(String folder) {
            try {
                return Directory.GetFiles(FilePath.Combine(Application.dataPath, folder), "*.txt");
            } catch {
                return new List<String>();
            }
        }

        private PackInfo LoadAllResourcesFolder(PackInfo pack) {
            var levels = Resources.LoadAll(pack.Folder);
            foreach (var level in levels) {
                var fileName = level.name;
                pack.AddLevelInfo(fileName);
            }
            return pack;
        }

        public static IList<PackInfo> GetPacks() {
            return Instance.packs;
        }

        // TODO How about storing current level in Levels (not in Preferences)?
        public static LevelInfo GetLevelByName(string levelName) {
            foreach (var pack in Instance.packs) {
                foreach (var level in pack.Levels) {
                    if (level.Path == levelName) {
                       return level;
                    }
                }
            }
            return null;
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
