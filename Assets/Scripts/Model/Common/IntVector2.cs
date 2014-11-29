namespace TrappedGame.Model.Common {
    public struct IntVector2 {
        public int y;
        public int x;

        public IntVector2(int x, int y) {
            this.x = x;
            this.y = y;
        }

        public void Set(int newX, int newY) {
            x = newX;
            y = newY;
        }

        public static bool operator == (IntVector2 left, IntVector2 right) {
            return left.x == right.x && left.y == right.y;
        }

        public static bool operator != (IntVector2 left, IntVector2 right) {
            return left.x != right.x || left.y != right.y;
        }

        public override bool Equals(object other) {
            if (!(other is IntVector2)) {
                return false;
            }
            var vector = (IntVector2) other;
            return x.Equals(vector.x) && y.Equals(vector.y);
        }
        
        // TODO non read only value in HashCode
        public override int GetHashCode() {
            return x.GetHashCode () ^ y.GetHashCode () << 2;
        }
    }
}