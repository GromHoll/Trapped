using TrappedGame.Model;
using TrappedGame.Utils;
using UnityEngine;

namespace TrappedGame.View.Graphic {
    public class PathGOFactory : MonoBehaviour {
        
        public GameObject pathFolder;
        public GameObject pathSegment;

        public GameObject CreatePathSegment(Path.PathLink link, Vector2 coord) {
            if (link.IsWentUp())    { coord.y += 0.5f; }
            if (link.IsWentRight()) { coord.x += 0.5f; }
            if (link.IsWentDown())  { coord.y -= 0.5f; }
            if (link.IsWentLeft())  { coord.x -= 0.5f; }
            
            var pathGameObject = GameUtils.InstantiateChild(pathSegment, coord, pathFolder);
            if (link.IsVertical()) {
                pathGameObject.transform.Rotate(0, 0, 90);
            }

            return pathGameObject;
        }
    }
}