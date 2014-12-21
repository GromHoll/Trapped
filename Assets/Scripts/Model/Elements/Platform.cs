using TrappedGame.Model.Common;

namespace TrappedGame.Model.Elements {
    public class Platform : MovableElement {

        public Platform(int x, int y) {
            position = new IntVector2(x, y);
        }

    }
}
