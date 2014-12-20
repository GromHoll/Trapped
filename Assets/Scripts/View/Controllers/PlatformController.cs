using TrappedGame.Model;
using TrappedGame.Model.Elements;
using TrappedGame.Utils;
using TrappedGame.View.Sync;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    class PlatformController : MonoBehaviour, ISyncGameObject {

        // TODO Rewrite setters
        public Platform Platform { get; set; }
        public Level Level { get; set; }

        void Update() {
            // TODO Update position 
            transform.position = GameUtils.ConvertToGameCoord(Platform.X, Platform.Y, Level);
        }

        public bool IsSync() {
            // TODO Check real position
            return true;
        }

    }
}
