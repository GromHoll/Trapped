using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using TrappedGame.Model;
using TrappedGame.Utils;
using UnityEngine;

namespace TrappedGame.View.Graphic {
    public class PathGOFactory : MonoBehaviour {

        private const string PATH_FOLDER = "Path/";
        
        public GameObject pathSegment;
        public GameObject pathCrossGameObject;
        public GameObject pathStart;

        public void CreatePathStart(Level level) {
            var coord = GameUtils.ConvertToGameCoord(level.StartX, level.StartY, level);
            var folder = GameObjectUtils.GetSubFolderByPath(gameObject, PATH_FOLDER);
            GameObjectUtils.InstantiateChild(pathStart, coord, folder);
        }

        public IList<GameObject> CreatePathSegment(Path.PathLink link, Level level) {
            return new List<GameObject> {
                CreateSegment(link, level),
                CreateCross(link, level)
            };
        }

        private GameObject CreateSegment(Path.PathLink link, Level level) {
            var folder = GameObjectUtils.GetSubFolderByPath(gameObject, PATH_FOLDER);
            var coord = GameUtils.ConvertToGameCoord(link.From, level);
            coord.y += link.IsWentUp() ? 0.5f : 0;
            coord.y += link.IsWentDown() ? -0.5f : 0;
            coord.x += link.IsWentLeft() ? -0.5f : 0;
            coord.x += link.IsWentRight() ? 0.5f : 0;

            var pathSegmentGameObject = GameObjectUtils.InstantiateChild(pathSegment, coord, folder);
            var rotation = link.IsHorizontal() ? 0 : 90;
            pathSegmentGameObject.transform.Rotate(0, 0, rotation);
            return pathSegmentGameObject;
        }

        private GameObject CreateCross(Path.PathLink link, Level level) {
            var previousLink = link.PreviousLink;
            if (previousLink == null || !previousLink.IsAdjacent()) { return null; }
            
            var folder = GameObjectUtils.GetSubFolderByPath(gameObject, PATH_FOLDER);
            var coord = GameUtils.ConvertToGameCoord(link.From, level);

            if (previousLink.IsHorizontal() && link.IsHorizontal()) {
                return GameObjectUtils.InstantiateChild(pathSegment, coord, folder);    
            }
            if (previousLink.IsVertical() && link.IsVertical()) {
                var straitLine = GameObjectUtils.InstantiateChild(pathSegment, coord, folder);
                straitLine.transform.Rotate(0, 0, 90);
                return straitLine;
            }

            var cross = GameObjectUtils.InstantiateChild(pathCrossGameObject, coord, folder);
            if (previousLink.IsWentUp()) {
                cross.transform.Rotate(0, 0, link.IsWentRight() ? 0 : 270);
            } else if (previousLink.IsWentDown()) {
                cross.transform.Rotate(0, 0, link.IsWentRight() ? 90 : 180);
            } else if (previousLink.IsWentLeft()) {
                cross.transform.Rotate(0, 0, link.IsWentUp() ? 90 : 0);
            } else {
                cross.transform.Rotate(0, 0, link.IsWentUp() ? 180 : 270);
            }
            
            return cross;
        } 

    }
}