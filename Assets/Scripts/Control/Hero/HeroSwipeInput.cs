using System;
using UnityEngine;

namespace TrappedGame {
    public class HeroSwipeInput : HeroInput {

        private Vector2 startPosition;
        private bool isStarted = false;

        public override HeroMovement GetMovement() {
            if (Input.touchCount == 0) {
                return noMovement;
            }

            var touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began) {
                startPosition = touch.position;
                isStarted = true;
            } else if (touch.phase == TouchPhase.Ended) {
                if (!isStarted) {
                    return noMovement;
                }

                Vector2 finishPosition = touch.position;
                Vector2 delta = finishPosition - startPosition;
                Debug.Log("Touch delta = " +  delta.x + ", " + delta.y);
                var angle = Math.Atan2(delta.y, delta.x) * 57.29578f;   //Vector2.Angle(Vector2.zero, direction);
                Debug.Log("Touch angle = " + angle);

                if (angle > -30 && angle < 30) {
                    return rightMovement;
                } else if (angle > 60 && angle < 120) {
                    return upMovement;
                } else if (angle > -120 && angle < -60) {
                    return downMovement;
                } else if (angle > 150 || angle < -150) {
                    return leftMovement;
                }
                isStarted = false;
            }
            return noMovement;
        }

    }
}

