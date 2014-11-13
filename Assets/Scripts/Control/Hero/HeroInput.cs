using System;
using UnityEngine;

namespace TrappedGame {
    public class HeroInput {

        private HeroMovement noMovement = new HeroMovement();
        private HeroMovement upMovement = new UpHeroMovement();
        private HeroMovement rightMovement = new RightHeroMovement();
        private HeroMovement downMovement = new DownHeroMovement();
        private HeroMovement leftMovement = new LeftHeroMovement();

        public HeroMovement GetMovement() {
            bool up    = Input.GetKeyDown(KeyCode.UpArrow);
            bool right = Input.GetKeyDown(KeyCode.RightArrow);
            bool down  = Input.GetKeyDown(KeyCode.DownArrow);
            bool left  = Input.GetKeyDown(KeyCode.LeftArrow);
            
            if (up)    return upMovement;
            if (right) return rightMovement;
            if (down)  return downMovement;
            if (left)  return leftMovement;
            return noMovement;
        }
    }
}