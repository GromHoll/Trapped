using TrappedGame.Model.Common;
using TrappedGame.Model.Elements;

namespace TrappedGame.Model {
    public class Hero : MovableElement {

        public bool IsDead { get; private set; }
        public int DeathCount { get; private set; }
        
        public Hero(int x, int y) {
            position = new IntVector2(x, y);
        }
        
        public void SetDead(bool isDead) {
            IsDead = isDead;
            if (IsDead) {
                DeathCount++;
            }
        }
    }
}