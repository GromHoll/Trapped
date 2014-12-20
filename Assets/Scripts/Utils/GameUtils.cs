using System;
using System.Linq;
using TrappedGame.Model;
using TrappedGame.Model.Common;

using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

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

        public static GameObject InstantiateChild(GameObject prefab, Vector2 vector, GameObject parent) {
            return InstantiateChildForWorld(prefab, vector, parent, true);
        }

        public static GameObject InstantiateChildForWorld(GameObject prefab, Vector2 vector, GameObject parent, bool worldStay) {
            var child = Object.Instantiate(prefab, vector, Quaternion.identity) as GameObject;
            if (child != null && parent != null) {
                child.transform.SetParent(parent.transform, worldStay);
            }
            return child;
        }

        public static GameObject GetSubFolderByPath(GameObject folder, string path) {
            var names = path.Split(new[] {'/', '\\', '.'}, StringSplitOptions.RemoveEmptyEntries);
            return names.Aggregate(folder, GetSubFolderByName);
        }

        public static GameObject GetSubFolderByName(GameObject folder, string name) {
            return FindChildByName(folder, name) ?? CreateEmptyChild(folder, name);
        }

        public static GameObject FindChildByName(GameObject parent, string name) {
            var transform = parent.transform.FindChild(name);
            return transform != null ? transform.gameObject : null;
        }

        public static GameObject CreateEmptyChild(GameObject parent, string name) {
            var result = new GameObject(name);
            result.transform.SetParent(parent.transform);
            return result;
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

