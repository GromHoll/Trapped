using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


namespace TrappedGame {
    public class LevelLoader {

        public Level LoadLevel(string fileName) {
            // TODO rewrite this shit
            TextAsset text = Resources.Load<TextAsset>(fileName);
            MemoryStream stream = new MemoryStream(text.bytes);
            StreamReader reader = new StreamReader(stream);

            LevelInfo levelInfo = ReadHeader(reader);
            ReadMap(reader, levelInfo);
            ReadDescription(reader, levelInfo);
            return levelInfo.CreateLevel();
        }

        private LevelInfo ReadHeader(StreamReader reader) {
            string levelName = reader.ReadLine();        
            string sizeLine = reader.ReadLine();
            string[] sizes = sizeLine.Split(' ');
            int xSize = int.Parse(sizes[0]);
            int ySize = int.Parse(sizes[1]);        
            return new LevelInfo(levelName, xSize, ySize);
        }

        private void ReadMap(StreamReader reader, LevelInfo levelInfo) {
            int xSize = levelInfo.GetXSize();
            int ySize = levelInfo.GetYSize();     
            for (int y = ySize - 1; y >= 0; y--) {
                string row = reader.ReadLine();
                for(int x = 0; x < xSize; x++) {
                    levelInfo.SetSymbol(x, y, row[x]);
                }
            }
        }

        private void ReadDescription(StreamReader reader, LevelInfo levelInfo) {
            string descriptionCountLine = reader.ReadLine();
            int descriptionCount = int.Parse(descriptionCountLine);        
            for (int i = 0; i < descriptionCount; i++) {
                string line = reader.ReadLine();
                string[] description = line.Split(':');   
                char symbol = description[0][0];
                levelInfo.AddDescription(symbol, description[1]);
            }
        }
    }
}