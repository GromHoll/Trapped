namespace TrappedGame.Model.Cells {
    public class WallCell : Cell {
     
        public WallCell(int x, int y) : base(x, y, CellType.WALL) {}

        public override bool IsBlocked() {
            return true;
        }
    }
}