using System.Linq;
using TrappedGame.Model.Common;

namespace TrappedGame.Model.Elements {
    public class MovableElement {

        protected IntVector2 position;
        public IntVector2 Coordinate { get { return position; } }
        public int X { get { return position.x; } }
        public int Y { get { return position.y; } }

        protected readonly Path path = new Path();
        public Path Path { get { return path; } }

        public void MoveTo(int x, int y) {
            path.AddLink(position.x, position.y, x, y);
            position.x = x;
            position.y = y;
        }

        public void MoveBack() {
            if (!path.IsEmpty()) {
                var link = path.RemoveLink();
                position.x = link.FromX;
                position.y = link.FromY;
            }
        }

        public Path.PathLink GetPreviousTurn() {
            return path.GetPreviousTurn();
        }

        public bool WasHere(int x, int y) {
            return path.Links.Any(link => link.FromX == x && link.FromY == y);
        }
    }
}
