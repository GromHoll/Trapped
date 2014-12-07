using System.Linq;
using TrappedGame.Model.Common;

namespace TrappedGame.Model {
    public class Hero {

        private IntVector2 position;
        public int X { get { return position.x; } }
        public int Y { get { return position.y; } }
        public bool IsDead { get; private set; }
        public int DeathCount { get; private set; }

        private readonly Path path = new Path();
        public Path Path { get { return path; } }

        public Hero(int x, int y) {
            position = new IntVector2(x, y);
        }

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
        
        public void SetDead(bool isDead) {
            IsDead = isDead;
            if (IsDead) {
                DeathCount++;
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