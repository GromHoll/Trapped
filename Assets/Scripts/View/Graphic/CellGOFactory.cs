using System;
using System.Collections.Generic;
using TrappedGame.Model;
using TrappedGame.Model.Cells;
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

        private IDictionary<Type, GameObject> cellPrefabs;

        void Start() {
            cellPrefabs = new Dictionary<Type, GameObject> {
                {typeof(EmptyCell), tilePrefab},   
                {typeof(WallCell), wallPrefab},   
                {typeof(SpearCell), spearPrefab},   
                {typeof(LaserCell), laserPrefab},   
                {typeof(UnknownCell), unknownPrefab},   
            };       
        }

        private GameObject CreateCellGameObject(Cell cell, Level level, GameObject folder) {
            var prefab = cellPrefabs[cell.GetType()];
            return CreateCellGameObject(cell, prefab, level, folder);
        }

        private GameObject CreateCellGameObject(Cell cell, GameObject prefab, Level level, GameObject folder) {
            var coord = GameUtils.ConvertToGameCoord(cell.Coordinate, level);
            return GameUtils.InstantiateChild(prefab, coord, folder);
        }
        
        public void CreateEmptyCells(Level level) {
            foreach (Cell cell in level.Cells) {
                CreateCellGameObject(cell, tilePrefab, level, emptyCellsFolder);
            }
        }

        public IList<SpearController> CreateSpearCells(Level level) {
            IList<SpearController> spears = new List<SpearController>();
            foreach (var spear in level.GetCells<SpearCell>()) {
                var spearObject = CreateCellGameObject(spear, level, spearCellsFolder);
                var controller = spearObject.GetComponent<SpearController>();
                controller.Cell = spear;
                spears.Add(controller);
            }
            return spears;
        }

        public void CreateWallCells(Level level) {
            foreach (var wall in level.GetCells<WallCell>()) {
                CreateCellGameObject(wall, level, wallCellsFolder);
            }
        }

        public void CreateLaserCells(Level level) {
            foreach (var laserCell in level.GetCells<LaserCell>()) {
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