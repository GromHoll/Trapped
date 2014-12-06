using TrappedGame.Model.Cells;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class IndicatorController : MonoBehaviour {
        
        public bool state;

        private CountCell cell;

        private void Update() {
            renderer.enabled = cell.IsOnOnNextTick() == state;
        }

        public void SetCell(CountCell newCell) {
            cell = newCell;
        }

    }
}
