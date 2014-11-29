using UnityEngine;

namespace TrappedGame.Control.Hero {
    public class HeroKeyInput : HeroInput {

        public override HeroMovement GetMovement() {
            var up    = Input.GetKeyDown(KeyCode.UpArrow);
            var right = Input.GetKeyDown(KeyCode.RightArrow);
            var down  = Input.GetKeyDown(KeyCode.DownArrow);
            var left  = Input.GetKeyDown(KeyCode.LeftArrow);
            
            if (up)    return UP_MOVEMENT;
            if (right) return RIGHT_MOVEMENT;
            if (down)  return DOWN_MOVEMENT;
            if (left)  return LEFT_MOVEMENT;
            return NO_MOVEMENT;
        }

    }
}

