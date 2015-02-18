using UnityEngine;

namespace TrappedGame.View.Controllers.Common {
    public abstract class MovableObjectController : MonoBehaviour {

        public float speed = 10;

        private Vector3 targetPosition;
        private Vector3 startPosition;
        private float timeElapsed;

        protected virtual void Start() {
            targetPosition = gameObject.transform.position;
            startPosition = gameObject.transform.position;
            timeElapsed = 0;
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
                timeElapsed += Time.deltaTime;
                gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, speed*timeElapsed);
            } else {
                targetPosition = GetNewPosition();
                startPosition = gameObject.transform.position;
                timeElapsed = 0;
            }
        }
    }
}
