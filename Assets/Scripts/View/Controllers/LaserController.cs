using TrappedGame.Model.Cells;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class LaserController : MonoBehaviour {
        public LaserCell Cell {
            set {
                foreach (var indicators in GetComponentsInChildren<Indicator4Controller>()) {
                    indicators.Cell = value;
                }
                foreach (var gun in GetComponentsInChildren<LaserGunController>()) {
                    gun.Cell = value;
                }   
            }
        }
    }
}
