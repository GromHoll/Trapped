using System.IO;
using UnityEngine;

namespace TrappedGame.Model.Loader {
    public class LevelLoader {

        public Level LoadLevel(string fileName) {
            // TODO rewrite this shit
            var text = Resources.Load<TextAsset>(fileName);
            var stream = new MemoryStream(text.bytes);
            var reader = new StreamReader(stream);

            var levelInfo = ReadHeader(reader);
            ReadMap(reader, levelInfo);
            ReadDescription(reader, levelInfo);
            return levelInfo.CreateLevel();
        }

        private LevelInfo ReadHeader(StreamReader reader) {
            var levelName = reader.ReadLine();        
            var sizeLine = reader.ReadLine();
            var sizes = sizeLine.Split(' ');
            var xSize = int.Parse(sizes[0]);
            var ySize = int.Parse(sizes[1]);        
            return new LevelInfo(levelName, xSize, ySize);
        }

        private void ReadMap(StreamReader reader, LevelInfo levelInfo) {
            var xSize = levelInfo.XSize;
            var ySize = levelInfo.YSize;     
            for (var y = ySize - 1; y >= 0; y--) {
                var row = reader.ReadLine();
                for(var x = 0; x < xSize; x++) {
                    levelInfo.SetSymbol(x, y, row[x]);
                }
            }
        }

        private void ReadDescription(StreamReader reader, LevelInfo levelInfo) {
            var descriptionCountLine = reader.ReadLine();
            var descriptionCount = int.Parse(descriptionCountLine);        
            for (var i = 0; i < descriptionCount; i++) {
                var line = reader.ReadLine();
                var description = line.Split(':');   
                var symbol = description[0][0];
                levelInfo.AddDescription(symbol, description[1]);
            }
        }
    }
}