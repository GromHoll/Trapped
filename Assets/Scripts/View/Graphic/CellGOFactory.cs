using System;
using System.Collections.Generic;
using TrappedGame.Model;
using TrappedGame.Model.Cells;
using TrappedGame.Utils;
using TrappedGame.View.Controllers;
using UnityEngine;

namespace TrappedGame.View.Graphic {
    public class CellGOFactory : MonoBehaviour {

        private const string MAP_FOLDER = "Map/";
        private const string PIT_CELLS_FOLDER = MAP_FOLDER + "PitCells";
        private const string WALL_CELLS_FOLDER = MAP_FOLDER + "WallCells";
        private const string EMPTY_CELLS_FOLDER = MAP_FOLDER + "EmptyCells";
        private const string SPEAR_CELLS_FOLDER = MAP_FOLDER + "SpearCells";
        private const string LASER_CELLS_FOLDER = MAP_FOLDER + "LaserCells";
        
        public GameObject emptyCellPrefab;
        public GameObject pitCellPrefab;
        public GameObject unknownPrefab;
        public GameObject laserPrefab;
        public GameObject laserLinePrefab;
        public GameObject wallPrefab;
        public GameObject spearPrefab;

        private IDictionary<Type, GameObject> cellPrefabs;

        void Start() {
            cellPrefabs = new Dictionary<Type, GameObject> {
                {typeof(EmptyCell), emptyCellPrefab},   
                {typeof(WallCell), wallPrefab},   
                {typeof(SpearCell), spearPrefab},   
                {typeof(LaserCell), laserPrefab},   
                {typeof(PitCell), pitCellPrefab},  
                {typeof(UnknownCell), unknownPrefab},  
            };       
        }

        private GameObject CreateCellGameObject(Cell cell, Level level, string folderPath) {
            var prefab = cellPrefabs[cell.GetType()];
            return CreateCellGameObject(cell, prefab, level, folderPath);
        }

        private GameObject CreateCellGameObject(Cell cell, GameObject prefab, Level level, string folderPath) {
            var folder = GameUtils.GetSubFolderByPath(gameObject, folderPath);
            var coord = GameUtils.ConvertToGameCoord(cell.Coordinate, level);
            return GameUtils.InstantiateChild(prefab, coord, folder);
        }
        
        public void CreateEmptyCells(Level level) {
            foreach (Cell cell in level.Cells) {
                CreateCellGameObject(cell, emptyCellPrefab, level, EMPTY_CELLS_FOLDER);
            }
        }

        public IList<SpearController> CreateSpearCells(Level level) {
            IList<SpearController> spears = new List<SpearController>();
            foreach (var spear in level.GetCells<SpearCell>()) {
                var spearObject = CreateCellGameObject(spear, level, SPEAR_CELLS_FOLDER);
                var controller = spearObject.GetComponent<SpearController>();
                controller.Cell = spear;
                spears.Add(controller);
            }
            return spears;
        }

        public void CreateWallCells(Level level) {
            foreach (var wall in level.GetCells<WallCell>()) {
                CreateCellGameObject(wall, level, WALL_CELLS_FOLDER);
            }
        }

        public void CreatePitCells(Level level) {
            foreach (var pit in level.GetCells<PitCell>()) {
                CreateCellGameObject(pit, level, PIT_CELLS_FOLDER);
            }
        }

        public void CreateLaserCells(Level level) {
            foreach (var laserCell in level.GetCells<LaserCell>()) {
                var laserObject = CreateCellGameObject(laserCell, level, LASER_CELLS_FOLDER);
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