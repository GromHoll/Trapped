using TrappedGame.Model.Cells;
using TrappedGame.View.Sync;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class SpearController : MonoBehaviour, ISyncGameObject {

        public static readonly string STATUS_KEY = "Status";

        private Animator aminator;

        private SpearCell cell;
        public SpearCell Cell {
            get { return cell; }
            set {
                cell = value;
                foreach (var indicators in GetComponentsInChildren<IndicatorController>()) {
                    indicators.Cell = value;
                }    
            }
        }

        void Start() {
            aminator = GetComponent<Animator>();
        }

        void Update() {
            aminator.SetBool(STATUS_KEY, GetStatus());
        }

        private bool GetStatus() {
            return cell != null && cell.IsDeadly();
        }

        public bool IsSync() {
            // TODO use constant
            if (aminator == null) return false;
            var currentState = aminator.GetCurrentAnimatorStateInfo(0);
            return currentState.IsName("Disabled") || currentState.IsName("Enabled");
        }
    }
}