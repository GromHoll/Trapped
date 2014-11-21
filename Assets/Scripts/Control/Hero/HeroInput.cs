using System;
using UnityEngine;

namespace TrappedGame {
    public abstract class HeroInput {

        protected HeroMovement noMovement = new HeroMovement();
        protected HeroMovement upMovement = new UpHeroMovement();
        protected HeroMovement rightMovement = new RightHeroMovement();
        protected HeroMovement downMovement = new DownHeroMovement();
        protected HeroMovement leftMovement = new LeftHeroMovement();

        public abstract HeroMovement GetMovement();
    }
}