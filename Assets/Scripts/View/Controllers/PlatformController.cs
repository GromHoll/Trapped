using TrappedGame.Model;
using TrappedGame.Model.Elements;
using TrappedGame.Utils;
using TrappedGame.View.Controllers.Common;
using TrappedGame.View.Sync;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    class PlatformController : MovableObjectController, ISyncGameObject {

        private Platform platform;
        private Level level;
        
        public void SerPlatform(Platform newPlatform, Level newLevel) {
            platform = newPlatform;
            level = newLevel;    
        }

        public bool IsSync() {
            return !IsMoving();     
        }

        protected override Vector3 GetNewPosition() {
            return GameUtils.ConvertToGameCoord(platform.X, platform.Y, level);
        }
    }
}
