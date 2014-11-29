using System;
using System.Collections.Generic;
using TrappedGame.Model.Cells;
using TrappedGame.Model.Common;
using TrappedGame.Model.LevelUtils;

namespace TrappedGame.Model.Loader {
    public class LevelInfo {

        public static readonly string DEFAULT_LEVEL_NAME = "Unknown";
        
        public static readonly char DEFAULT_START = 's';
        public static readonly char DEFAULT_BONUS = 'b';
        public static readonly char DEFAULT_FINISH = 'f';
        
        public static readonly char DEFAULT_TIME_0 = '0';
        public static readonly char DEFAULT_TIME_2 = '2';
        
        private readonly string levelName;
        private readonly int xSize;
        private readonly int ySize;
        private readonly char[,] symbols;
        private readonly IDictionary<char, string> descriptions = new Dictionary<char, string>();
        
        public LevelInfo(string levelName, int xSize, int ySize) {
            if (xSize <= 0) throw new ArgumentException("Size should be positive", "xSize");
            if (ySize <= 0) throw new ArgumentException("Size should be positive", "ySize");
            this.levelName = levelName ?? DEFAULT_LEVEL_NAME;
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
            var levelBuilder = new LevelBuilder(levelName, xSize, ySize);
            var cellFacrory = new CellFactory();
            
            for(var x = 0; x < xSize; x++) {
                for (var y = 0; y < ySize; y++) {
                    var symbol = symbols[x, y];
                    if (symbol == DEFAULT_START) {
                        levelBuilder.SetStart(x, y);
                    } else if (symbol == DEFAULT_FINISH) {
                        levelBuilder.SetFinish(x, y);
                    } else if (symbol == DEFAULT_BONUS) {
                        levelBuilder.AddBonus(new IntVector2(x, y));
                    } else if (symbol == DEFAULT_TIME_0) {
                        levelBuilder.AddTimeBonus(new IntVector2(x, y), LevelTick.FREEZE_LEVEL_TICK);
                    } else if (symbol == DEFAULT_TIME_2) {
                        levelBuilder.AddTimeBonus(new IntVector2(x, y), new LevelTick(2));
                    }
                    
                    Cell cell;
                    if(descriptions.ContainsKey(symbols[x, y])) {
                        var description = descriptions[symbols[x, y]];
                        cell = cellFacrory.GetCellByDescription(description, x, y);
                    } else {
                        cell = cellFacrory.GetCellBySymbol(symbols[x, y], x, y);
                    }                
                    levelBuilder.AddCell(cell);
                }
            }        
            return levelBuilder.Build();
        }        
    }
}