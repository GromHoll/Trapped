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
            return CreateCellGameObject(cell.GetCoordinate(), cell.GetCellType(), level, folder);
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
            foreach (Cell cell in level.GetCells()) {
                if (cell.GetCellType() != CellType.WALL) {
                    CreateCellGameObject(cell.GetCoordinate(), CellType.EMPTY, level, emptyCellsFolder);
                }
            }
        }

        public void CreateSpearCells(Level level) {
            foreach (Cell cell in level.GetCells()) {
                //TODO Add to level different cell accesses
                if (cell.GetCellType() == CellType.SPEAR) {
                    var spearObject = CreateCellGameObject(cell, level, spearCellsFolder);
                    var controller = spearObject.GetComponent<SpearController>();
                    var spear = (SpearCell) cell;
                    controller.SetCell(spear);
                }
            }
        }

        public void CreateWallCells(Level level) {
            foreach (Cell cell in level.GetCells()) {
                if (cell.GetCellType() == CellType.WALL) {
                    CreateCellGameObject(cell, level, wallCellsFolder);
                }
            }
        }

        public void CreateLaserCells(Level level) {
            foreach (Cell cell in level.GetCells()) {
                if (cell.GetCellType() == CellType.LASER) {
                    var laserObject = CreateCellGameObject(cell, level, laserCellsFolder);
                    var controller = laserObject.GetComponent<LaserController>();
                    var laser = (LaserCell) cell;
                    controller.SetCell(laser);
                    foreach (var line in laser.GetLaserLines()) {
                        CreateLaserLinesForLaser(level, line, laserObject);  
                    }
                }
            }
        }

        private void CreateLaserLinesForLaser(Level level, LaserCell.Laser line, GameObject laser) {
            var cover = line.GetCover();
            var coord = GameUtils.ConvertToGameCoord(cover.GetMinX(), cover.GetMinY(), level);
            var laserObject = GameUtils.InstantiateChild(laserLinePrefab, coord, laser);
            var controller = laserObject.GetComponent<LaserLineController>();
            controller.SetLaserLine(line);
        }

    }
}