namespace TrappedGame.Model.Cells {
    public class SpearCell : CountCell {

        public SpearCell(int x, int y, int onPeriod, int offPeriod, int currentTick, bool isOn)
            : base(x, y, CellType.SPEAR, onPeriod, offPeriod, currentTick, isOn) {}
        
        public SpearCell(int x, int y)
            : base(x, y, CellType.SPEAR) {}

        public override bool IsDeadly() {
            return IsOn;
        }
    }
}

