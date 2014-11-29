namespace TrappedGame.Control.Hero {

    public class HeroMovement {
        public virtual void MoveHeroInGame(Game game) {}
    }

    public class UpHeroMovement : HeroMovement {
        public override void MoveHeroInGame(Game game) {
            game.MoveHeroUp();
        }
    }

    public class RightHeroMovement : HeroMovement {
        public override void MoveHeroInGame(Game game) {
            game.MoveHeroRight();
        }
    }

    public class DownHeroMovement : HeroMovement {
        public override void MoveHeroInGame(Game game) {
            game.MoveHeroDown();
        }
    }

    public class LeftHeroMovement : HeroMovement {
        public override void MoveHeroInGame(Game game) {
            game.MoveHeroLeft();
        }
    }
}