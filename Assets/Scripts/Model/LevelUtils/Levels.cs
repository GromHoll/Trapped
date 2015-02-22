using System.Collections.Generic;
using System.Collections.ObjectModel;
using TrappedGame.Utils;

namespace TrappedGame.Model.LevelUtils {
	public class Levels {

	    public class LevelInfo {
            public PackInfo Pack { get; internal set; }
            public string Source { get; private set; }
            public string Name { get; private set; }

            public LevelInfo(string source, string name = null) {
                Validate.NotNullOrEmpty(source, "Source can't be a null or empty");
                Source = source;
                Name = name ?? source;
            }
	    }

        public class PackInfo {
            public string Name { get; private set; }
            public string Folder { get; private set; }
            public IList<LevelInfo> Levels { get; private set; }

            public PackInfo(string name, string folder, IList<LevelInfo> levels) {
                Validate.NotNullOrEmpty(name, "Name can't be a null or empty");
                Validate.NotNullOrEmpty(folder, "Folder can't be a null or empty");
                Validate.NotNull(levels, "Levels can't be a null");
                Name = name;
                Folder = folder;
                Levels = levels;
                foreach (var levelInfo in levels) {
                    levelInfo.Pack = this;
                }
            } 
	    }



        private static readonly Levels Instance = new Levels();

	    private Levels() {
	        
	    }


	}
}
