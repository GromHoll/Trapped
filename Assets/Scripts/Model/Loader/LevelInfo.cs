using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TrappedGame {
    public class LevelInfo {

        public static readonly string DEFAULT_LEVEL_NAME = "Unknown";
        
        public static readonly char DEFAULT_START = 's';
        public static readonly char DEFAULT_BONUS = 'b';
        public static readonly char DEFAULT_FINISH = 'f';
        
        public static readonly char DEFAULT_TIME_0 = '0';
        public static readonly char DEFAULT_TIME_2 = '2';
        
        private string levelName;
        private int xSize;
        private int ySize;
        private char[,] symbols;
        private IDictionary<char, string> descriptions = new Dictionary<char, string>();
        
        public LevelInfo(string levelName, int xSize, int ySize) {
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
        
        // TODO refactor
        public Level CreateLevel() {
            LevelBuilder LevelBuilder = new LevelBuilder(levelName, xSize, ySize);
            CellFactory cellFacrory = new CellFactory();
            
            for(int x = 0; x < xSize; x++) {
                for (int y = 0; y < ySize; y++) {
                    char symbol = symbols[x, y];
                    if (symbol == DEFAULT_START) {
                        LevelBuilder.SetStart(x, y);
                    } else if (symbol == DEFAULT_FINISH) {
                        LevelBuilder.SetFinish(x, y);
                    } else if (symbol == DEFAULT_BONUS) {
                        LevelBuilder.AddBonus(new IntVector2(x, y));
                    } else if (symbol == DEFAULT_TIME_0) {
                        LevelBuilder.AddTimeBonus(new IntVector2(x, y), LevelTick.FREEZE_LEVEL_TICK);
                    } else if (symbol == DEFAULT_TIME_2) {
                        LevelBuilder.AddTimeBonus(new IntVector2(x, y), new LevelTick(2));
                    }
                    
                    Cell cell;
                    if(descriptions.ContainsKey(symbols[x, y])) {
                        string description = descriptions[symbols[x, y]];
                        cell = cellFacrory.getCellByDescription(description, x, y);
                    } else {
                        cell = cellFacrory.getCellBySymbol(symbols[x, y], x, y);
                    }                
                    LevelBuilder.AddCell(cell);
                }
            }        
            return LevelBuilder.Build();
        }        
    }
}

