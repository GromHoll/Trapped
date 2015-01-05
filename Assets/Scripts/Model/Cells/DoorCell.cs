namespace TrappedGame.Model.Cells {
    class DoorCell : Cell {

        public DoorCell(int x, int y) : base(x, y) {}

        public override bool IsBlocked() {
            // TODO check keys
            return true;
        }
    }
}
