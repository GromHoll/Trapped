using TrappedGame.Model;
using TrappedGame.Model.Cells;
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
            disabledState = FindChildByName("DisabledState");
            passiveState = FindChildByName("PassiveState");
            activeState = FindChildByName("ActiveState");

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

        // TODO Maybe move to Utils
        private GameObject FindChildByName(string childName) {
            return transform.FindChild(childName).gameObject;
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
