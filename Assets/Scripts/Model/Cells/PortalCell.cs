using TrappedGame.Model.Common;

namespace TrappedGame.Model.Cells {
    class PortalCell : Cell {

        public PortalCell Pair { get; protected set; }

        public PortalCell(int x, int y) : base(x, y) {
            Pair = null;
        }

        public PortalCell(int x, int y, PortalCell pairedPortal) : base(x, y) {
            this.Pair = pairedPortal;
            pairedPortal.Pair = this;
        }

        public IntVector2 EndPoint(IntVector2 income) {
            var result = Pair.Coordinate.Clone();
            result.x += Coordinate.x - income.x;
            result.y += Coordinate.y - income.y;
            return result;
        }
    }
}
