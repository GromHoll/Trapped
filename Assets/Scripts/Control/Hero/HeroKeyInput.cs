using System;
using UnityEngine;

namespace TrappedGame {
    public class HeroKeyInput : HeroInput {

        public override HeroMovement GetMovement() {
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

