using TrappedGame.Model.Common;

namespace TrappedGame.Model.Elements {
    public class Platform {

        public IntVector2 Coordinate { get; protected set; }
        public int X { get { return Coordinate.x; } }
        public int Y { get { return Coordinate.y; } }

        public Platform(int x, int y) {
            Coordinate = new IntVector2(x, y);
        }

    }
}
