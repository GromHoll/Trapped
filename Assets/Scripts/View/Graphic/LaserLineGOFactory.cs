using TrappedGame.Model.Cells;
using TrappedGame.Model.Common;
using UnityEngine;
using System.Collections.Generic;

namespace TrappedGame {
    public class LaserLineGOFactory : MonoBehaviour {

        public GameObject laserLineFolder;
        public GameObject laserLine;

        public IDictionary<LaserCell.Laser, IList<GameObject>> CreateLasersForLevel(Level level) {
            IDictionary<LaserCell.Laser, IList<GameObject>> lasers = new Dictionary<LaserCell.Laser, IList<GameObject>>();
            foreach (LaserCell.Laser line in level.GetLaserLines()) {
                IList<GameObject> laserObjects = new List<GameObject>();
                IntRect cover = line.GetCover();
                for (int x = cover.GetMinX(); x <= cover.GetMaxX(); x++) {
                    for (int y = cover.GetMinY(); y <= cover.GetMaxY(); y++) {
                        GameObject laserObject = CreateLaserLine(line, x, y, level);
                        laserObjects.Add(laserObject);                        
                    }
                }
                lasers.Add(line, laserObjects);
            }
            return lasers;
        }      
        
        private GameObject CreateLaserLine(LaserCell.Laser line, int x, int y, Level level) {
            Vector2 coord = GameUtils.ConvertToGameCoord(x, y, level);
            GameObject laserObject = GameUtils.InstantiateChild(laserLine, coord, laserLineFolder);
            laserObject.SetActive(line.IsDanger());    
            if (line.IsHorizontal()) {
                laserObject.transform.Rotate(0, 0, 90);
            }
            return laserObject;
        }
    }
}

