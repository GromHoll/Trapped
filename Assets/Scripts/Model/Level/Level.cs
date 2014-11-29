using System;
using TrappedGame.Model.Cells;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace TrappedGame {
    public class Level : ITickable {
        
        private string name;
        
        private IntVector2 size;
        private Cell[,] cells;

        private IList<LaserCell.Laser> lasers = new List<LaserCell.Laser>();

        private IntVector2 start;
        private IntVector2 finish;
        private IList<IntVector2> bonuses;
        private IDictionary<IntVector2, LevelTick> timeBonuses;

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
            LaserHelper laserHelper = new LaserHelper();
            foreach (Cell cell in cells) {
                // TODO maybe store LaserCell in specias list and don't use cast?
                if(cell.GetCellType() == CellType.LASER) {
                    LaserCell laser = (LaserCell) cell;
                    addLaser(laserHelper.CreateUpLaser(laser, this));
                    addLaser(laserHelper.CreateRightLaser(laser, this));
                    addLaser(laserHelper.CreateDownLaser(laser, this));
                    addLaser(laserHelper.CreateLeftLaser(laser, this));
                }
            }
        }

        private void addLaser(LaserCell.Laser laser) {
            if (laser != null) {
                lasers.Add(laser);
            }
        }

        public void NextTick() {
            foreach (Cell cell in cells) {
                cell.NextTick();
            }
        }

        public void BackTick() {
            foreach (Cell cell in cells) {
                cell.BackTick();
            }
        }

        public LevelTick GetLevelTick(int x, int y) {
            IntVector2 coord = new IntVector2(x, y);
            return timeBonuses.ContainsKey(coord) ? timeBonuses[coord] : LevelTick.DEFAULT_LEVEL_TICK;
        }

        public bool IsDangerCell(int x, int y) {            
            Cell cell = GetCell(x, y);
            if(cell.IsDeadly()) {
                return true;
            }
            foreach (LaserCell.Laser laser in lasers) {
                if (laser.IsDangerFor(x, y)) {
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

        public IList<LaserCell.Laser> GetLaserLines() {
            return lasers;
        }

        public bool Contains(int x, int y) {
            return x >= 0 && x <= size.x - 1 && y >= 0 && y <= size.y - 1; 
        }

    }
}
