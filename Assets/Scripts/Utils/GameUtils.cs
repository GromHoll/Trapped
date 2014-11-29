using TrappedGame.Model;
using TrappedGame.Model.Common;
using UnityEngine;

namespace TrappedGame.Utils {
    public static class GameUtils {

        public static Vector2 ConvertToGameCoord(IntVector2 pos, Level level) {
            return ConvertToGameCoord(pos.x, pos.y, level);
        }

        public static Vector2 ConvertToGameCoord(float x, float y, Level level) {
            var gameX = x - (level.GetSizeX() - 1)/2f;
            var gameY = y - (level.GetSizeY() - 1)/2f;
            return new Vector2(gameX, gameY);
        }

        public static GameObject InstantiateChild(GameObject gameObject, Vector2 vector, GameObject parent) {
            var child = Object.Instantiate(gameObject, vector, Quaternion.identity) as GameObject;
            if (child != null && parent != null) {
                child.transform.parent = parent.transform;
            }
            return child;
        }
    }
}

