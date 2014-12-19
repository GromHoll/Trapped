using System;
using TrappedGame.Model;
using TrappedGame.Model.Cells;
using TrappedGame.View.Sync;
using UnityEngine;

namespace TrappedGame.View.Graphic {
    class CellGraphicInfo {
        public GameObject Prefab { get; private set; }
        public bool IsNeedsEmptyCell { get; private set; }
        public string FolderName { get; private set; }
        public Func<Cell, Level, CellGraphicInfo, ISyncGameObject> CreateFunction  { get; private set; }

        public CellGraphicInfo(GameObject prefab, Func<Cell, Level, CellGraphicInfo, ISyncGameObject> createFunction,
                               bool isNeedsEmptyCell, string folderName) {
            Prefab = prefab;
            IsNeedsEmptyCell = isNeedsEmptyCell;
            FolderName = folderName;
            CreateFunction = createFunction;
        }
    }
}
