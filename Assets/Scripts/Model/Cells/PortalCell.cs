using TrappedGame.Utils;

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

    }
}
