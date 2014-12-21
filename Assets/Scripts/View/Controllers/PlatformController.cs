using TrappedGame.Model;
using TrappedGame.Model.Elements;
using TrappedGame.Utils;
using TrappedGame.View.Sync;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    class PlatformController : MonoBehaviour, ISyncGameObject {

        private Platform platform;
        private Level level;
        
        // TODO Union moving with heroController
        private Vector3 targetPosition;
        public float speed = 10;

        void Start() {
            targetPosition = gameObject.transform.position;
        }

        void Update() {
            UpdatePosition();
        }

        public void SerPlatform(Platform newPlatform, Level newLevel) {
            platform = newPlatform;
            level = newLevel;    
        }

        public bool IsSync() {
            return targetPosition == gameObject.transform.position;    
        }

        private void UpdatePosition() {
            if (!IsSync()) {
                var currentPosition = gameObject.transform.position;
                var direction = targetPosition - currentPosition;
                direction.Normalize();
                direction *= speed * Time.deltaTime;

                var newPosition = gameObject.transform.position + direction;
                if (Vector3.Distance(targetPosition, newPosition) >= Vector3.Distance(targetPosition, currentPosition))
                {
                    newPosition = targetPosition;
                }
                gameObject.transform.position = newPosition;
            } else {
                targetPosition = GameUtils.ConvertToGameCoord(platform.X, platform.Y, level);
            }
        }
    }
}
