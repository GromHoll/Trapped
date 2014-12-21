using TrappedGame.Model.Cells;
using TrappedGame.Utils;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class LaserGunController : MonoBehaviour {
        public enum Direction {
            UP = 1,
            RIGHT = 2,
            DOWN = 3,
            LEFT = 4
        }

        private bool isEnable;
        private GameObject disabledState;
        private GameObject passiveState;
        private GameObject activeState;
        
        public int direction;
        public LaserCell Cell { get; set; }

        private void Start() {
            isEnable = IsEnable();
            disabledState = GameObjectUtils.FindChildByName(gameObject, "DisabledState");
            passiveState = GameObjectUtils.FindChildByName(gameObject, "PassiveState");
            activeState = GameObjectUtils.FindChildByName(gameObject, "ActiveState");

            disabledState.SetActive(!isEnable);
            passiveState.SetActive(isEnable);
            activeState.SetActive(isEnable);
        }

        private void Update() {
            if (isEnable) {
                passiveState.SetActive(!Cell.IsOn);
                activeState.SetActive(Cell.IsOn);
            }
        }

        private bool IsEnable() {
            switch (direction) {
                case (int) Direction.UP:
                    return Cell.Up;
                case (int) Direction.RIGHT:
                    return Cell.Right;
                case (int) Direction.DOWN:
                    return Cell.Down;
                case (int) Direction.LEFT:
                    return Cell.Left;
            }
            return false;
        }
    }
}
