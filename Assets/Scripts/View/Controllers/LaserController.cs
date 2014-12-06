using TrappedGame.Model.Cells;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class LaserController : MonoBehaviour {

        private LaserCell cell;

        public void SetCell(LaserCell newCell) {
            cell = newCell;
            foreach (var indicators in GetComponentsInChildren<IndicatorController>()) {
                indicators.SetCell(cell);
            }
        }
    }
}
