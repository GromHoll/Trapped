using TrappedGame.Model.Common;

namespace TrappedGame.Model.Elements {
    public class Platform {

        private IntVector2 position;
        public IntVector2 Coordinate { get { return position; } }
        public int X { get { return position.x; } }
        public int Y { get { return position.y; } }

        private readonly Path path = new Path();

        public Platform(int x, int y) {
            position = new IntVector2(x, y);
        }

        // TODO Union with hero
        public void MoveTo(int x, int y) {
            path.AddLink(position.x, position.y, x, y);
            position.x = x;
            position.y = y;
        }

        // TODO Union with hero
        public void MoveBack() {
            if (!path.IsEmpty()) {
                var link = path.RemoveLink();
                position.x = link.FromX;
                position.y = link.FromY;
            }
        }

    }
}
