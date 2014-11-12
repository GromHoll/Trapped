using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class LevelLoader {

    public Level LoadLevel(string fileName) {
        // TODO change file loading to Assets access
        StreamReader reader = File.OpenText(fileName);

        LevelBuilder levelBuilder = ReadHeader(reader);
        ReadMap(reader, levelBuilder);
        ReadDescription(reader, levelBuilder);
        return levelBuilder.Build();
    }

    private LevelBuilder ReadHeader(StreamReader reader) {
        string levelName = reader.ReadLine();        
        string sizeLine = reader.ReadLine();
        string[] sizes = sizeLine.Split(' ');
        int xSize = int.Parse(sizes[0]);
        int ySize = int.Parse(sizes[1]);        
        return new LevelBuilder(levelName, xSize, ySize);
    }

    private void ReadMap(StreamReader reader, LevelBuilder levelBuilder) {
        int xSize = levelBuilder.GetXSize();
        int ySize = levelBuilder.GetYSize();     
        for (int y = ySize - 1; y >= 0; y--) {
            string row = reader.ReadLine();
            for(int x = 0; x < xSize; x++) {
                levelBuilder.SetSymbol(x, y, row[x]);
            }
        }
    }

    private void ReadDescription(StreamReader reader, LevelBuilder levelBuilder) {
        string descriptionCountLine = reader.ReadLine();
        int descriptionCount = int.Parse(descriptionCountLine);        
        for (int i = 0; i < descriptionCount; i++) {
            string line = reader.ReadLine();
            string[] description = line.Split(':');   
            char symbol = description[0][0];
            levelBuilder.AddDescription(symbol, description[1]);
        }
    }

}
