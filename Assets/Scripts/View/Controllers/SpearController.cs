using TrappedGame.Model.Cells;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class SpearController : MonoBehaviour {

        public static readonly string STATUS_KEY = "Status";

        private Animator aminator;
        private SpearCell cell;

        void Start() {
            aminator = GetComponent<Animator>();
        }

        void Update() {
            aminator.SetBool(STATUS_KEY, GetStatus());
        }

        public void SetCell(SpearCell cell) {
            this.cell = cell;
        }

        private bool GetStatus() {
            return cell != null && cell.IsDeadly();
        }
    }
}