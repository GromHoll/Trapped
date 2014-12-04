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

        public IDictionary<SpearCell, GameObject> CreateSpearCells(Level level) {
            IDictionary<SpearCell, GameObject> spearsCells = new Dictionary<SpearCell, GameObject>();
            foreach (Cell cell in level.GetCells()) {
                if (cell.GetCellType() == CellType.SPEAR) {
                    var spearObject = CreateCellGameObject(cell, level, spearCellsFolder);
                    var controller = spearObject.GetComponent<SpearController>();
                    //TODO Add to level different cell accesses
                    var spear = (SpearCell) cell;
                    controller.SetCell(spear);
                    spearsCells[spear] = spearObject;
                }
            }
            return spearsCells;
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
                    CreateCellGameObject(cell, level, laserCellsFolder);
                }
            }
        }
    }
}