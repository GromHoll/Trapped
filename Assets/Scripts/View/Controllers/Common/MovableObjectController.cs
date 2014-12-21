using UnityEngine;

namespace TrappedGame.View.Controllers.Common {
    public abstract class MovableObjectController : MonoBehaviour {

        private Vector3 targetPosition;
        public float speed = 10;

        protected virtual void Start() {
            targetPosition = gameObject.transform.position;
        }

        protected virtual void Update() {
            UpdatePosition();
        }
        
        protected bool IsMoving() {
            return targetPosition != gameObject.transform.position;
        }

        protected abstract Vector3 GetNewPosition();

        private void UpdatePosition() {
            if (IsMoving()) {
                var currentPosition = gameObject.transform.position;
                var direction = targetPosition - currentPosition;
                direction.Normalize();
                direction *= speed * Time.deltaTime;

                var newPosition = gameObject.transform.position + direction;
                if (Vector3.Distance(targetPosition, newPosition) >= Vector3.Distance(targetPosition, currentPosition)) {
                    newPosition = targetPosition;
                }
                gameObject.transform.position = newPosition;
            } else {
                targetPosition = GetNewPosition();
            }
        }
    }
}
