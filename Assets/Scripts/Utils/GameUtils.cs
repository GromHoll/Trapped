using System.Collections.Generic;
using TrappedGame.Model;
using TrappedGame.Model.Common;
using UnityEngine;

namespace TrappedGame.Utils {
    class GameUtils {
        public static Vector2 ConvertToGameCoord(IntVector2 pos, Level level) {
            return ConvertToGameCoord(pos.x, pos.y, level);
        }

        public static Vector2 ConvertToGameCoord(float x, float y, Level level) {
            var gameX = x - (level.SizeX - 1)/2f;
            var gameY = y - (level.SizeY - 1)/2f;
            return new Vector2(gameX, gameY);
        }

        public static void AddIfNotNull<T>(ICollection<T> list, T element) {
            if (element != null) {
                list.Add(element);   
            }
        }
    }
}
