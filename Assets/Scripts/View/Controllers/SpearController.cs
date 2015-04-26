using TrappedGame.Model.Cells;
using TrappedGame.View.Sync;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class SpearController : MonoBehaviour, ISyncGameObject {

        private const string STATUS_KEY = "Status";
        private const string DISABLED_NAME = "Disabled";
        private const string ENABLED_NAME = "Enabled";

        private Animator aminator;

        private SpearCell cell;
        public SpearCell Cell {
            get { return cell; }
            set {
                cell = value;
                foreach (var indicators in GetComponentsInChildren<Indicator4Controller>()) {
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
            if (aminator == null) return false;
            var currentState = aminator.GetCurrentAnimatorStateInfo(0);
            return currentState.IsName(DISABLED_NAME) || currentState.IsName(ENABLED_NAME);
        }
    }
}