using TrappedGame.Model.Common;

namespace TrappedGame.Model.Cells {
    public class Cell : ITickable {

        public IntVector2 Coordinate { get; protected set; }
        public int X { get { return Coordinate.x; } }
        public int Y { get { return Coordinate.y; } }
        public CellType CellType { get; protected set; }
       
        public Cell(int x, int y) 
        : this(x, y, CellType.EMPTY) {}

        public Cell(int x, int y, CellType type) {
            Coordinate = new IntVector2(x, y);
            CellType = type;
        }

        public virtual bool IsBocked() {
            return false;
        }

        public virtual bool IsDeadly() {
            return false;
        }

        public virtual bool IsDeadlyFor(int x, int y) {
            return false;
        }

        public virtual void NextTick() {}
        public virtual void BackTick() {}
    }
}
