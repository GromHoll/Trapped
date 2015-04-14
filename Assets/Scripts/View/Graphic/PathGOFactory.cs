using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using TrappedGame.Model;
using TrappedGame.Model.Common;
using TrappedGame.Utils;
using TrappedGame.View.Controllers;
using UnityEngine;

namespace TrappedGame.View.Graphic {
    public class PathGOFactory : MonoBehaviour {

        private const string PATH_FOLDER = "Path/";

        public GameObject startPathLink;
        public GameObject crossPathLink;
        public GameObject straightPathLine;

        public PathLinkController CreateLink(Path.PathLink link, HeroController hero) {
            var pathGameObject = CreatePathLink(link, hero.Level);
            var pathLinkController = pathGameObject.GetComponent<PathLinkController>();
            pathLinkController.HeroController = hero;
            pathLinkController.PathLink = link;
            return pathLinkController;
        }

        private GameObject CreatePathLink(Path.PathLink link, Level level) {
            /* Hardcoded path to parent */
            var parent = gameObject.transform.parent.parent.gameObject;
            var folder = GameObjectUtils.GetSubFolderByPath(parent, PATH_FOLDER);
            var coord = GameUtils.ConvertToGameCoord(link.From, level);
            var previousLink = link.PreviousLink;
            if (previousLink == null) {
                return GameObjectUtils.InstantiateChild(startPathLink, coord, folder);
            }
            if (previousLink.IsAdjacent()) {
                if (previousLink.IsHorizontal() && link.IsVertical() || previousLink.IsVertical() && link.IsHorizontal()) {
                    return GameObjectUtils.InstantiateChild(crossPathLink, coord, folder);
                }
            }
            return GameObjectUtils.InstantiateChild(straightPathLine, coord, folder);
        }
    }
}