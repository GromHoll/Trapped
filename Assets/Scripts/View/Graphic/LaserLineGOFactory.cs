using System.Collections.Generic;
using TrappedGame.Model;
using TrappedGame.Model.Cells;
using TrappedGame.Model.Common;
using TrappedGame.Utils;
using UnityEngine;

namespace TrappedGame.View.Graphic {
    public class LaserLineGOFactory : MonoBehaviour {

        public GameObject laserLineFolder;
        public GameObject laserLine;

        public IDictionary<LaserCell.Laser, IList<GameObject>> CreateLasersForLevel(Level level) {
            var lasers = new Dictionary<LaserCell.Laser, IList<GameObject>>();
            foreach (var line in level.GetLaserLines()) {
                var laserObjects = new List<GameObject>();
                var cover = line.GetCover();
                for (var x = cover.GetMinX(); x <= cover.GetMaxX(); x++) {
                    for (var y = cover.GetMinY(); y <= cover.GetMaxY(); y++) {
                        var laserObject = CreateLaserLine(line, x, y, level);
                        laserObjects.Add(laserObject);                        
                    }
                }
                lasers.Add(line, laserObjects);
            }
            return lasers;
        }      
        
        private GameObject CreateLaserLine(LaserCell.Laser line, int x, int y, Level level) {
            var coord = GameUtils.ConvertToGameCoord(x, y, level);
            var laserObject = GameUtils.InstantiateChild(laserLine, coord, laserLineFolder);
            laserObject.SetActive(line.IsDanger());    
            if (line.IsVertical()) {
                laserObject.transform.Rotate(0, 0, 90);
            }
            return laserObject;
        }
    }
}