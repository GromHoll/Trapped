using System.Linq;
using TrappedGame.Model.Common;
using TrappedGame.Utils.Observer;

namespace TrappedGame.Model.Elements {
    public class MovableElement {

        protected IntVector2 position;
        public IntVector2 Coordinate { get { return position; } }
        public int X { get { return position.x; } }
        public int Y { get { return position.y; } }

        protected readonly Path path = new Path();
        public Path Path { get { return path; } }

        private readonly SimpleSubject<Path.PathLink> movementSubject = new SimpleSubject<Path.PathLink>();
        public SimpleSubject<Path.PathLink> MovementSubject { get { return movementSubject; } }

        public void MoveTo(int x, int y) {
            var link = path.AddLink(position.x, position.y, x, y);
            position.x = x;
            position.y = y;
            movementSubject.NotifyObservers(link);
        }

        public void MoveBack() {
            if (!path.IsEmpty()) {
                var link = path.RemoveLink();
                position.x = link.FromX;
                position.y = link.FromY;
                movementSubject.NotifyObservers(link.Reverse());
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
