using System;
using UnityEngine;

namespace TrappedGame.Control.Hero {
    public class HeroSwipeInput : HeroInput {

        private Vector2 startPosition;
        private bool isStarted = false;

        public override HeroMovement GetMovement() {
            if (Input.touchCount == 0) {
                return NO_MOVEMENT;
            }

            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                startPosition = touch.position;
                isStarted = true;
            } else if (touch.phase == TouchPhase.Ended) {
                if (!isStarted) {
                    return NO_MOVEMENT;
                }

                var finishPosition = touch.position;
                var delta = finishPosition - startPosition;
                var angle = Math.Atan2(delta.y, delta.x) * 57.29578f;
                
                if (angle > -30 && angle < 30) {
                    return RIGHT_MOVEMENT;
                } else if (angle > 60 && angle < 120) {
                    return UP_MOVEMENT;
                } else if (angle > -120 && angle < -60) {
                    return DOWN_MOVEMENT;
                } else if (angle > 150 || angle < -150) {
                    return LEFT_MOVEMENT;
                }
                isStarted = false;
            }
            return NO_MOVEMENT;
        }
    }
}