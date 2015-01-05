using System;
using System.Collections.Generic;
using System.Linq;
using TrappedGame.Model;
using TrappedGame.Model.Cells;
using TrappedGame.Utils;
using TrappedGame.View.Controllers;
using TrappedGame.View.Sync;
using UnityEngine;

namespace TrappedGame.View.Graphic {
    public class CellGOFactory : MonoBehaviour {
        
        private const string MAP_FOLDER = "Map/";
        private const string PIT_CELLS_FOLDER = MAP_FOLDER + "PitCells";
        private const string WALL_CELLS_FOLDER = MAP_FOLDER + "WallCells";
        private const string DOOR_CELLS_FOLDER = MAP_FOLDER + "DoorCells";
        private const string EMPTY_CELLS_FOLDER = MAP_FOLDER + "EmptyCells";
        private const string SPEAR_CELLS_FOLDER = MAP_FOLDER + "SpearCells";
        private const string LASER_CELLS_FOLDER = MAP_FOLDER + "LaserCells";
        private const string PORTAL_CELLS_FOLDER = MAP_FOLDER + "PortalCells";
        private const string UNKNOWN_CELLS_FOLDER = MAP_FOLDER + "UnknownCells";
        
        public GameObject unknownPrefab;
        public GameObject portalCellPrefab;
        public GameObject emptyCellPrefab;
        public GameObject laserLinePrefab;
        public GameObject laserPrefab;
        public GameObject spearPrefab;
        public GameObject wallPrefab;
        public GameObject doorPrefab;
        public GameObject pitPrefab;

        private IDictionary<Type, CellGraphicInfo> cellGraphicInfo;

        void Start() {
            cellGraphicInfo = new Dictionary<Type, CellGraphicInfo> {
                {typeof(EmptyCell),   new CellGraphicInfo(emptyCellPrefab, CreateSimpleCell, false, EMPTY_CELLS_FOLDER)},   
                {typeof(WallCell),    new CellGraphicInfo(wallPrefab, CreateSimpleCell, false, WALL_CELLS_FOLDER)},   
                {typeof(SpearCell),   new CellGraphicInfo(spearPrefab, CreateSpearCell, true, SPEAR_CELLS_FOLDER)},   
                {typeof(LaserCell),   new CellGraphicInfo(laserPrefab, CreateLaserCell, true, LASER_CELLS_FOLDER)},   
                {typeof(PitCell),     new CellGraphicInfo(pitPrefab, CreateSimpleCell, false, PIT_CELLS_FOLDER)}, 
                {typeof(PortalCell),  new CellGraphicInfo(portalCellPrefab, CreateSimpleCell, true, PORTAL_CELLS_FOLDER)},
                {typeof(DoorCell),    new CellGraphicInfo(doorPrefab, CreateSimpleCell, true, DOOR_CELLS_FOLDER)},
                {typeof(UnknownCell), new CellGraphicInfo(unknownPrefab, CreateSimpleCell, false, UNKNOWN_CELLS_FOLDER)},   
            }; 
        }

        public IList<ISyncGameObject> CreateLevel(Level level) {
            return level.Cells.Cast<Cell>()
                        .Select(cell => CreateCell(cell, level))
                        .Where(syncGO => syncGO != null)
                        .ToList();
        }

        private ISyncGameObject CreateCell(Cell cell, Level level) {
            var graphicInfo = cellGraphicInfo[cell.GetType()];
            if (graphicInfo.IsNeedsEmptyCell) {
                var emptyGraphicInfo = cellGraphicInfo[typeof(EmptyCell)];
                CreateCellGameObject(cell, level, emptyGraphicInfo);
            }
            return graphicInfo.CreateFunction(cell, level, graphicInfo);
        }

        private GameObject CreateCellGameObject(Cell cell, Level level, CellGraphicInfo graphicInfo) {
            var folder = GameObjectUtils.GetSubFolderByPath(gameObject, graphicInfo.FolderName);
            var coord = GameUtils.ConvertToGameCoord(cell.Coordinate, level);
            return GameObjectUtils.InstantiateChild(graphicInfo.Prefab, coord, folder);
        }
        
        private ISyncGameObject CreateSimpleCell(Cell cell, Level level, CellGraphicInfo graphicInfo) {
            CreateCellGameObject(cell, level, graphicInfo);
            return null;
        }

        private ISyncGameObject CreateSpearCell(Cell cell, Level level, CellGraphicInfo graphicInfo) {
            var spearCell = cell as SpearCell;
            var spearObject = CreateCellGameObject(spearCell, level, graphicInfo);
            var controller = spearObject.GetComponent<SpearController>();
            controller.Cell = spearCell;
            return controller;
        }

        private ISyncGameObject CreateLaserCell(Cell cell, Level level, CellGraphicInfo graphicInfo) {
            var laserCell = cell as LaserCell;
            var laserObject = CreateCellGameObject(laserCell, level, graphicInfo);
            var controller = laserObject.GetComponent<LaserController>();
            controller.Cell = laserCell;
            foreach (var line in laserCell.LaserLines) {
                CreateLaserLinesForLaser(level, line, laserObject);
            }
            return null;
        }

        private void CreateLaserLinesForLaser(Level level, LaserCell.Line line, GameObject laser) {
            var cover = line.Cover;
            var coord = GameUtils.ConvertToGameCoord(cover.MinX, cover.MinY, level);
            var laserObject = GameObjectUtils.InstantiateChild(laserLinePrefab, coord, laser);
            var controller = laserObject.GetComponent<LaserLineController>();
            controller.Line = line;
        }
    }
}