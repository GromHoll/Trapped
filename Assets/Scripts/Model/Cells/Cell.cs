using TrappedGame.Model.Common;

namespace TrappedGame.Model.Cells {
    public abstract  class Cell : ITickable {

        public IntVector2 Coordinate { get; protected set; }
        public int X { get { return Coordinate.x; } }
        public int Y { get { return Coordinate.y; } }
       
        protected Cell(int x, int y) {
            Coordinate = new IntVector2(x, y);
        }

        public virtual bool IsBlocked() {
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
