using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TrappedGame {
    public class LevelBuilder {

        private string name;
        
        private IntVector2 size;
        private Cell[,] cells;
        
        private IntVector2 start;
        private IntVector2 finish;
        private IList<IntVector2> bonuses = new List<IntVector2>();
        private IDictionary<IntVector2, LevelTick> timeBonuses = new Dictionary<IntVector2, LevelTick>();

        public LevelBuilder(string name, int xSize, int ySize) {
            if (xSize <= 0) throw new ArgumentException("Size should be positive", "xSize");
            if (ySize <= 0) throw new ArgumentException("Size should be positive", "ySize");
            
            size = new IntVector2(xSize, ySize);
            cells = new Cell[size.x, size.y];
            for (int x = 0; x < size.x; x++) {
                for (int y = 0; y < size.y; y++) {
                    AddCell(new Cell(x, y, CellType.UNKNOWN));
                }
            }
        }

        public void AddCell(Cell cell) {
            int x = cell.GetX();
            int y = cell.GetY();
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