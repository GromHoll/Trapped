using UnityEngine;

namespace TrappedGame {
    public class Cell : ITickable {

        protected IntVector2 coordinate;
        protected CellType cellType;
       
        public Cell(int x, int y) 
        : this(x, y, CellType.EMPTY) {}

        public Cell(int x, int y, CellType type) {
            this.coordinate = new IntVector2(x, y);
            this.cellType = type;
        }

        public virtual bool IsBocked() {
            return false;
        }

        public virtual bool IsDeadly() {
            return false;
        }

        public IntVector2 GetCoordinate() {
            return coordinate;
        }

        public int GetX() {
            return coordinate.x;
        }

        public int GetY() {
            return coordinate.y;
        }
        
        public CellType GetCellType() {
            return cellType;
        }

        public virtual void NextTick() {}
        public virtual void BackTick() {}
    }
}
