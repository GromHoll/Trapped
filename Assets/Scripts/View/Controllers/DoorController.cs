using TrappedGame.Model.Cells;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class DoorController : MonoBehaviour {
        public DoorCell Cell { get; set; }

        private void Update() {
            renderer.enabled = Cell.IsBlocked();
        }
    }
}
