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
        
        public Level(LevelBuilder builder) {
            size = builder.GetSize();
            start = builder.GetStart();
            finish = builder.GetFinish();
            cells = builder.GetCells();
            bonuses = builder.GetBonuses();
            timeBonuses = builder.GetTimeBonuses();
            CreateDangerZones();
        }

        private void CreateDangerZones() {
            CreateLasers();
        }

        // TODO Refactor
        private void CreateLasers() {
            var laserHelper = new LaserHelper();
            foreach (var cell in cells) {
                // TODO maybe store LaserCell in specias list and don't use cast?
                if(cell.GetCellType() == CellType.LASER) {
                    var laser = (LaserCell) cell;
                    laser.AddLaserLine(laserHelper.CreateUpLaser(laser, this));
                    laser.AddLaserLine(laserHelper.CreateRightLaser(laser, this));
                    laser.AddLaserLine(laserHelper.CreateDownLaser(laser, this));
                    laser.AddLaserLine(laserHelper.CreateLeftLaser(laser, this));
                }
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
            if (cell.IsDeadly()) {
                return true;
            }
            foreach (var cell2 in cells) {
                if (cell2.IsDeadlyFor(x, y)) {
                    return true;
                }
            }
            return false;
        }

        public int GetStartX() {
            return start.x;
        }

        public int GetStartY() {
            return start.y;
        }

        public int GetFinishX() {
            return finish.x;
        }
        
        public int GetFinishY() {
            return finish.y;
        }

        public int GetSizeX() {
            return size.x;
        }
        
        public int GetSizeY() {
            return size.y;
        }

        public Cell GetCell(int x, int y) {
            if (!Contains(x, y)) throw new ArgumentException("Wrong coordinates");
            return cells[x, y];
        }

        public IEnumerable GetCells() {
            return cells;
        }

        public IList<IntVector2> GetBonuses() {
            return bonuses;
        }

        public IDictionary<IntVector2, LevelTick> GetTimeBonuses() {
            return timeBonuses;
        }
        
        public bool Contains(int x, int y) {
            return x >= 0 && x <= size.x - 1 && y >= 0 && y <= size.y - 1; 
        }
    }
}