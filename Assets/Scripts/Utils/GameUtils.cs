using TrappedGame.Model;
using TrappedGame.Model.Common;

using UnityEngine;
using System.Collections.Generic;


namespace TrappedGame.Utils {
    public static class GameUtils {

        public static Vector2 ConvertToGameCoord(IntVector2 pos, Level level) {
            return ConvertToGameCoord(pos.x, pos.y, level);
        }

        public static Vector2 ConvertToGameCoord(float x, float y, Level level) {
            var gameX = x - (level.SizeX - 1)/2f;
            var gameY = y - (level.SizeY - 1)/2f;
            return new Vector2(gameX, gameY);
        }

        public static GameObject InstantiateChild(GameObject gameObject, Vector2 vector, GameObject parent) {
            return InstantiateChildForWorld(gameObject, vector, parent, true);
        }

        public static GameObject InstantiateChildForWorld(GameObject gameObject, Vector2 vector, GameObject parent, bool worldStay) {
            var child = Object.Instantiate(gameObject, vector, Quaternion.identity) as GameObject;
            if (child != null && parent != null) {
                child.transform.SetParent(parent.transform, worldStay);
            }
            return child;
        }

		public static void DestroyAllChild(GameObject parent) {
			var children = new List<GameObject>();
			foreach (Transform child in parent.transform) {
					children.Add(child.gameObject);
			}
			children.ForEach(child => Object.Destroy(child));
		}
    }
}

