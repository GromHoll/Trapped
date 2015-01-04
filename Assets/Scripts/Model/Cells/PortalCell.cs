using TrappedGame.Utils;

namespace TrappedGame.Model.Cells {
    class PortalCell : Cell {

        public class PortalKey {

            private readonly string key;

            public PortalKey(string newKey) {
                Validate.NotNull(newKey, "Key can't be null");
                key = newKey;
            }

            public static bool operator == (PortalKey left, PortalKey right) {
                return left != null && right != null && left.key == right.key;
            }

            public static bool operator != (PortalKey left, PortalKey right) {
                return left == null || right == null || left.key != right.key;
            }

            public override bool Equals(object other) {
                if (!(other is PortalKey)) { return false; }
                var portalKey = other as PortalKey;
                return key.Equals(portalKey.key);
            }

            public override int GetHashCode() {
                return key.GetHashCode();
            }
        }

        public PortalKey Key { get; protected set; }

        public PortalCell(int x, int y, PortalKey key) : base(x, y) {
            Key = key;
        }

    }
}
