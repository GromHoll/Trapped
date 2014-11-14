using System;
using UnityEngine;
using System.Collections.Generic;

namespace TrappedGame {
    public class Level : ITickable {
        
        private string name;
        
        private IntVector2 size;
        private Cell[,] cells;

        private List<LaserCell.Laser> lasers = new List<LaserCell.Laser>();

        private IntVector2 start;
        private IntVector2 finish;
        private IList<IntVector2> bonuses;

        public Level(LevelBuilder builder) {
            size = builder.GetSize();
            start = builder.GetStart();
            finish = builder.GetFinish();
            cells = builder.GetCells();
            bonuses = builder.GetBonuses();
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

                    var up = laserHelper.CreateUpLaser(laser, this);
                    addLaser(up);

                    var right = laserHelper.CreateRightLaser(laser, this);
                    addLaser(right);

                    var down = laserHelper.CreateDownLaser(laser, this);
                    addLaser(down);

                    var left = laserHelper.CreateLeftLaser(laser, this);
                    addLaser(left);
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
            if (x < 0 || x >= size.x) throw new ArgumentException("Size should be positive and less than xSize", "x");
            if (y < 0 || y >= size.y) throw new ArgumentException("Size should be positive and less than ySize", "y");
            return cells[x, y];
        }

        public IList<IntVector2> GetBonuses() {
            return bonuses;
        }

        public bool contains(int x, int y) {
            return x >= 0 && x <= size.x - 1 && y >= 0 && y <= size.y - 1; 
        }

        // TODO Remove convector methods after refactor
        public Vector2 ConvertToGameCoord(IntVector2 pos) {
            return ConvertToGameCoord(pos.x, pos.y);
        }

        public Vector2 ConvertToGameCoord(float x, float y) {
            float gameX = x - (size.x - 1)/2f;
            float gameY = y - (size.y - 1)/2f;
            return new Vector2(gameX, gameY);
        }
    }
}
