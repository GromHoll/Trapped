using System.Collections.Generic;
using TrappedGame.Control.Hero;
using TrappedGame.Utils;

namespace TrappedGame.Main {

    public class InputQueue {

        private LinkedList<HeroMovement> movements = new LinkedList<HeroMovement>();

        public bool HasNext() {
            return movements.Count != 0;
        }

        public void AddMovement(HeroMovement movement) {
            Validate.NotNull(movement);
            if (HeroInput.NO_MOVEMENT == movement) { return; }

            var lastNode = movements.Last;
            if (lastNode == null) {
                movements.AddLast(movement);
                return;
            }

            var last = lastNode.Value;
            if ((HeroInput.DOWN_MOVEMENT == last && HeroInput.UP_MOVEMENT == movement)
                || (HeroInput.UP_MOVEMENT == last && HeroInput.DOWN_MOVEMENT == movement)
                || (HeroInput.LEFT_MOVEMENT == last && HeroInput.RIGHT_MOVEMENT == movement)
                || (HeroInput.RIGHT_MOVEMENT == last && HeroInput.LEFT_MOVEMENT == movement)) {
                movements.RemoveLast();
                return;
            }

            movements.AddLast(movement);
        }

        public HeroMovement GetNextMovement() {
            var firstNode = movements.First;
            if (firstNode != null) {
                var result = firstNode.Value;
                movements.RemoveFirst();
                return result;
            } else {
                return HeroInput.NO_MOVEMENT;
            }
        }
    }
}


