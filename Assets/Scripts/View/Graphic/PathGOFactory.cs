using TrappedGame.Model;
using UnityEngine;
using System.Collections.Generic;

namespace TrappedGame {
    public class PathGOFactory : MonoBehaviour {
        
        public GameObject pathFolder;
        public GameObject pathSegment;

        public GameObject CreatePathSegment(Path.PathLink link, Vector2 coord) {
            if (link.IsWentUp())    { coord.y += 0.5f; }
            if (link.IsWentRight()) { coord.x += 0.5f; }
            if (link.IsWentDown())  { coord.y -= 0.5f; }
            if (link.IsWentLeft())  { coord.x -= 0.5f; }
            GameObject pathGameObject = GameUtils.InstantiateChild(pathSegment, coord, pathFolder);
            if (link.IsVertical()) {
                pathGameObject.transform.Rotate(0, 0, 90);
            }

            return pathGameObject;
        }

    }
}

