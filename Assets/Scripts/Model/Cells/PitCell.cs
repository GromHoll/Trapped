namespace TrappedGame.Model.Cells {
    public class PitCell : Cell {

        public PitCell(int x, int y) : base(x, y) {}

        public override bool IsDeadly() {
            return true;
        }

    }
}
