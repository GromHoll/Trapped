using UnityEngine;

namespace TrappedGame {
    public class CellGOFactory : MonoBehaviour {
        public GameObject tilePrefab;
        public GameObject unknownPrefab;
        public GameObject laserPrefab;
        public GameObject wallPrefab;
        public GameObject spearOnPrefab;
        public GameObject spearOffPrefab;

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
    }
}

