using TrappedGame.Model;
using TrappedGame.Utils;
using UnityEngine;

namespace TrappedGame.View.Graphic {
    public class PathGOFactory : MonoBehaviour {
        
        public GameObject pathFolder;
        public GameObject pathSegment;

        public GameObject CreatePathSegment(Path.PathLink link, Vector2 coord) {
            var pathGameObject = GameObjectUtils.InstantiateChild(pathSegment, coord, pathFolder);
            var rotation = link.IsWentUp() ? 180
                : link.IsWentRight() ? 90
                    : link.IsWentDown() ? 0
                        : -90;
            pathGameObject.transform.Rotate(0, 0, rotation);
            return pathGameObject;
        }
    }
}