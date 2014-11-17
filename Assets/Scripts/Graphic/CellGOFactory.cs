using UnityEngine;
using System.Collections.Generic;

namespace TrappedGame {
    public class CellGOFactory : MonoBehaviour {

        public GameObject emptyCellsFolder;
        public GameObject spearCellsFolder;
        public GameObject laserCellsFolder;
        public GameObject wallCellsFolder;

        public GameObject tilePrefab;
        public GameObject unknownPrefab;
        public GameObject laserPrefab;
        public GameObject wallPrefab;
        public GameObject spearOnPrefab;
        public GameObject spearOffPrefab;


        private GameObject CreateCellGameObject(Cell cell, Level level, GameObject folder) {
            return CreateCellGameObject(cell.GetCoordinate(), cell.GetCellType(), level, folder);
        }

        private GameObject CreateCellGameObject(IntVector2 cellCoord, CellType cellType, Level level, GameObject folder) {
            GameObject prefab = GetCellPrefab(cellType);
            Vector2 coord = GameUtils.ConvertToGameCoord(cellCoord.x, cellCoord.y, level);
            return GameUtils.InstantiateChild(prefab, coord, folder);
        }

        // TODO can be union with GetCellPrefab(CellType type) when spear will have single prefab
        public GameObject GetCellPrefab(Cell cell) {
            if (cell.GetCellType() == CellType.SPEAR) {
                return cell.IsDeadly() ? spearOnPrefab : spearOffPrefab;
            } else {
                return GetCellPrefab(cell.GetCellType());
            }
        }

        public GameObject GetCellPrefab(CellType type) {
            switch (type) {
            case CellType.EMPTY: 
                return tilePrefab;
            case CellType.UNKNOWN: 
                return unknownPrefab;
            case CellType.LASER: 
                return laserPrefab;
            case CellType.SPEAR: 
                return spearOnPrefab;
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
                    GameObject spearObject = CreateCellGameObject(cell, level, spearCellsFolder);
                    //TODO Add to level different cell accesses
                    SpearCell spear = (SpearCell) cell;
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

