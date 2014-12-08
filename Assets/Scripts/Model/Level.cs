using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TrappedGame.Model.Cells;
using TrappedGame.Model.Common;
using TrappedGame.Model.LevelUtils;

namespace TrappedGame.Model {
    public class Level : ITickable {
        
        private string name;

        private readonly Cell[,] cells; 
        public IEnumerable Cells { get { return cells; } }
        
        private IntVector2 size;
        public int SizeX { get { return size.x; } }
        public int SizeY { get { return size.y; } }
        
        private IntVector2 start;
        public int StartX { get { return start.x; } }
        public int StartY { get { return start.y; } } 

        private IntVector2 finish;
        public int FinishX { get { return finish.x; } }
        public int FinishY { get { return finish.y; } }

        public IList<IntVector2> Bonuses { get; private set; }
        public IList<LaserCell> LaserCells { get; private set; }
        public IDictionary<IntVector2, LevelTick> TimeBonuses { get; private set; }
        
        public Level(LevelBuilder builder) {
            size = builder.GetSize();
            start = builder.GetStart();
            finish = builder.GetFinish();
            cells = builder.GetCells();

            LaserCells = builder.GetLaserCells();
            Bonuses = builder.GetBonuses();
            TimeBonuses = builder.GetTimeBonuses();

            CreateDangerZones();
        }

        private void CreateDangerZones() {
            CreateLasers();
        }

        private void CreateLasers() {
            var laserHelper = new LaserHelper();
            foreach (var laser in LaserCells) {
                laser.CreateLaserLines(laserHelper, this);
            }
        }
        
        public void NextTick() {
            foreach (var cell in cells) {
                cell.NextTick();
            }
        }

        public void BackTick() {
            foreach (var cell in cells) {
                cell.BackTick();
            }
        }

        public LevelTick GetLevelTick(int x, int y) {
            var coord = new IntVector2(x, y);
            return TimeBonuses.ContainsKey(coord) ? TimeBonuses[coord] : LevelTick.DEFAULT_LEVEL_TICK;
        }

        public bool IsDangerCell(int x, int y) {            
            var cell = GetCell(x, y);
            return cell.IsDeadly() || LaserCells.Any(laser => laser.IsDeadlyFor(x, y));
        }

        public Cell GetCell(int x, int y) {
            if (!Contains(x, y)) throw new ArgumentException("Wrong coordinates");
            return cells[x, y];
        }

        public bool Contains(int x, int y) {
            return x >= 0 && x <= size.x - 1 && y >= 0 && y <= size.y - 1; 
        }
        
    }
}