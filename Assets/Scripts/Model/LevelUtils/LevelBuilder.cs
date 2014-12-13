using System;
using System.Collections.Generic;
using TrappedGame.Model.Cells;
using TrappedGame.Model.Common;

namespace TrappedGame.Model.LevelUtils {
    public class LevelBuilder {

        private string name;
        
        private readonly IntVector2 size;
        private readonly Cell[,] cells;
        
        private IntVector2 start;
        private IntVector2 finish;
        private readonly IList<IntVector2> bonuses = new List<IntVector2>();
        private readonly IDictionary<IntVector2, LevelTick> timeBonuses = new Dictionary<IntVector2, LevelTick>();
        
        public LevelBuilder(string name, int xSize, int ySize) {
            if (xSize <= 0) throw new ArgumentException("Size should be positive", "xSize");
            if (ySize <= 0) throw new ArgumentException("Size should be positive", "ySize");
            
            size = new IntVector2(xSize, ySize);
            cells = new Cell[size.x, size.y];
            for (var x = 0; x < size.x; x++) {
                for (var y = 0; y < size.y; y++) {
                    AddCell(new Cell(x, y, CellType.UNKNOWN));
                }
            }
        }

        public void AddCell(Cell cell) {
            var x = cell.X;
            var y = cell.Y;
            cells[x, y] = cell;
        }

        public void AddBonus(IntVector2 coordinate) {
            bonuses.Add(coordinate);
        }
        
        public void AddTimeBonus(IntVector2 coordinate, LevelTick levelTick) {
            timeBonuses.Add(coordinate, levelTick);
        }

        public void SetStart(int x, int y) {
            start.Set(x, y);
        }
        
        public void SetFinish(int x, int y) {
            finish.Set(x, y);
        }
        
        public IntVector2 GetSize() {
            return size;
        }

        public IntVector2 GetStart() {
            return start;
        }

        public IntVector2 GetFinish() {
            return finish;
        }

        public Cell[,] GetCells() {
            return cells;
        }
        
        public IList<IntVector2> GetBonuses() {
            return bonuses;
        }

        public IDictionary<IntVector2, LevelTick> GetTimeBonuses() {
            return timeBonuses;
        }

        public Level Build() {
            return new Level(this);
        }              
    }
}