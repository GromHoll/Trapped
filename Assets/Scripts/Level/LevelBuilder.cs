using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelBuilder {

    public static readonly string DEFAULT_LEVEL_NAME = "Unknown";

    public static readonly char DEFAULT_START = 's';
    public static readonly char DEFAULT_BONUS = 'b';
    public static readonly char DEFAULT_FINISH = 'f';

    private string levelName;
    private int xSize;
    private int ySize;
    private char[,] symbols;
    private IDictionary<char, string> descriptions = new Dictionary<char, string>();

    public LevelBuilder(string levelName, int xSize, int ySize) {
        if (xSize <= 0) throw new ArgumentException("Size should be positive", "xSize");
        if (ySize <= 0) throw new ArgumentException("Size should be positive", "ySize");
        this.levelName = (levelName != null) ? levelName : DEFAULT_LEVEL_NAME;
        this.symbols = new char[xSize, ySize];
        this.xSize = xSize;
        this.ySize = ySize;
    }

    public int GetXSize() {
        return xSize;
    }
    
    public int GetYSize() {
        return ySize;
    }

    public void SetSymbol(int x, int y, char symbol) {
        if (x < 0 || x >= xSize) throw new ArgumentException("Position should be positive and less than xSize", "x");
        if (y < 0 || y >= ySize) throw new ArgumentException("Position should be positive and less than ySize", "y");
        symbols[x,y] = symbol;
    }

    public void AddDescription(char symbol, string description) {
        descriptions.Add(symbol, description);
    }

    public Level Build() {
        // TODO refactor
        Level level = new Level(levelName, xSize, ySize);
        
        for(int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++) {
                char symbol = symbols[x, y];
                if (symbol == DEFAULT_START) {
                    level.SetStart(x, y);
                } else if (symbol == DEFAULT_FINISH) {
                    level.SetFinish(x, y);
                } else if (symbol == DEFAULT_BONUS) {
                    level.AddBonus(new IntVector2(x, y));
                }

                Cell cell;
                if(descriptions.ContainsKey(symbols[x, y])) {
                    string description = descriptions[symbols[x, y]];
                    cell = CellFactory.getCellByDescription(description, new Vector2(x, y));
                } else {
                    cell = CellFactory.getCellBySymbol(symbols[x, y], new Vector2(x, y));
                }                
                level.AddCell(cell);
            }
        }        
        return level;
    }
    
}

