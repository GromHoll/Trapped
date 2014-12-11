using System.Collections.Generic;
using TrappedGame.Model;
using TrappedGame.Model.Cells;
using TrappedGame.Model.Common;
using TrappedGame.Utils;
using TrappedGame.View.Controllers;
using UnityEngine;

namespace TrappedGame.View.Graphic {
    public class CellGOFactory : MonoBehaviour {

        public GameObject emptyCellsFolder;
        public GameObject spearCellsFolder;
        public GameObject laserCellsFolder;
        public GameObject wallCellsFolder;

        public GameObject tilePrefab;
        public GameObject unknownPrefab;
        public GameObject laserPrefab;
        public GameObject laserLinePrefab;
        public GameObject wallPrefab;
        public GameObject spearPrefab;


        private GameObject CreateCellGameObject(Cell cell, Level level, GameObject folder) {
            return CreateCellGameObject(cell.Coordinate, cell.CellType, level, folder);
        }

        private GameObject CreateCellGameObject(IntVector2 cellCoord, CellType cellType, Level level, GameObject folder) {
            var prefab = GetCellPrefab(cellType);
            var coord = GameUtils.ConvertToGameCoord(cellCoord.x, cellCoord.y, level);
            return GameUtils.InstantiateChild(prefab, coord, folder);
        }
        
        private GameObject GetCellPrefab(CellType type) {
            switch (type) {
            case CellType.EMPTY: 
                return tilePrefab;
            case CellType.UNKNOWN: 
                return unknownPrefab;
            case CellType.LASER: 
                return laserPrefab;
            case CellType.SPEAR: 
                return spearPrefab;
            case CellType.WALL:  
                return wallPrefab;
            default:
                return unknownPrefab;              
            }
        }

        public void CreateEmptyCells(Level level) {
            foreach (Cell cell in level.Cells) {
                if (cell.CellType != CellType.WALL) {
                    CreateCellGameObject(cell.Coordinate, CellType.EMPTY, level, emptyCellsFolder);
                }
            }
        }

        public IList<SpearController> CreateSpearCells(Level level) {
            IList<SpearController> spears = new List<SpearController>();
            foreach (Cell cell in level.Cells) {
                //TODO Add to level different cell accesses
                if (cell.CellType == CellType.SPEAR) {
                    var spearObject = CreateCellGameObject(cell, level, spearCellsFolder);
                    var controller = spearObject.GetComponent<SpearController>();
                    controller.Cell = (SpearCell) cell;
                    spears.Add(controller);
                }
            }
            return spears;
        }

        public void CreateWallCells(Level level) {
            foreach (Cell cell in level.Cells) {
                if (cell.CellType == CellType.WALL) {
                    CreateCellGameObject(cell, level, wallCellsFolder);
                }
            }
        }

        public void CreateLaserCells(Level level) {
            foreach (var laserCell in level.LaserCells) {
                var laserObject = CreateCellGameObject(laserCell, level, laserCellsFolder);
                var controller = laserObject.GetComponent<LaserController>();
                controller.Cell = laserCell;
                foreach (var line in laserCell.LaserLines) {
                    CreateLaserLinesForLaser(level, line, laserObject);  
                }
            }
        }

        private void CreateLaserLinesForLaser(Level level, LaserCell.Line line, GameObject laser) {
            var cover = line.Cover;
            var coord = GameUtils.ConvertToGameCoord(cover.MinX, cover.MinY, level);
            var laserObject = GameUtils.InstantiateChild(laserLinePrefab, coord, laser);
            var controller = laserObject.GetComponent<LaserLineController>();
            controller.Line = line;
        }

    }
}