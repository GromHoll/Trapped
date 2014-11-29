namespace TrappedGame.Control.Hero {
    public abstract class HeroInput {
        public static readonly HeroMovement NO_MOVEMENT = new HeroMovement();
        public static readonly HeroMovement UP_MOVEMENT = new UpHeroMovement();
        public static readonly HeroMovement RIGHT_MOVEMENT = new RightHeroMovement();
        public static readonly HeroMovement DOWN_MOVEMENT = new DownHeroMovement();
        public static readonly HeroMovement LEFT_MOVEMENT = new LeftHeroMovement();

        public abstract HeroMovement GetMovement();
    }
}