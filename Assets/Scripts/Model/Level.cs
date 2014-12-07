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
        
        private IntVector2 size;
        private readonly Cell[,] cells;

        private IntVector2 start;
        private IntVector2 finish;

        private readonly IList<IntVector2> bonuses;
        private readonly IDictionary<IntVector2, LevelTick> timeBonuses;

        private readonly IList<LaserCell> lasers;

        public Level(LevelBuilder builder) {
            size = builder.GetSize();
            start = builder.GetStart();
            finish = builder.GetFinish();

            cells = builder.GetCells();
            lasers = builder.GetLaserCells();
            
            bonuses = builder.GetBonuses();
            timeBonuses = builder.GetTimeBonuses();
            CreateDangerZones();
        }

        private void CreateDangerZones() {
            CreateLasers();
        }

        private void CreateLasers() {
            var laserHelper = new LaserHelper();
            foreach (var laser in lasers) {
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
            return timeBonuses.ContainsKey(coord) ? timeBonuses[coord] : LevelTick.DEFAULT_LEVEL_TICK;
        }

        public bool IsDangerCell(int x, int y) {            
            var cell = GetCell(x, y);
            return cell.IsDeadly() || lasers.Any(laser => laser.IsDeadlyFor(x, y));
        }

        public Cell GetCell(int x, int y) {
            if (!Contains(x, y)) throw new ArgumentException("Wrong coordinates");
            return cells[x, y];
        }

        public bool Contains(int x, int y) {
            return x >= 0 && x <= size.x - 1 && y >= 0 && y <= size.y - 1; 
        }

        public int StartX {
            get { return start.x; }
        }

        public int StartY {
            get { return start.y; }
        }

        public int FinishX {
            get { return finish.x; }
        }

        public int FinishY {
            get { return finish.y; }
        }

        public int SizeX {
            get { return size.x; }
        }

        public int SizeY {
            get { return size.y; }
        }

        public IEnumerable Cells {
            get { return cells; }
        }

        public IList<LaserCell> LaserCells {
            get { return lasers; }
        }

        public IList<IntVector2> Bonuses {
            get { return bonuses; }
        }

        public IDictionary<IntVector2, LevelTick> TimeBonuses {
            get { return timeBonuses; }
        }
    }
}