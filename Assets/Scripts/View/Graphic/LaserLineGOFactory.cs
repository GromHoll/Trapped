using System.Collections.Generic;
using System.Xml;
using TrappedGame.Model;
using TrappedGame.Model.Cells;
using TrappedGame.Utils;
using UnityEngine;

namespace TrappedGame.View.Graphic {
    public class LaserLineGOFactory : MonoBehaviour {

        public GameObject laserLineFolder;
        public GameObject laserLine;

        public IDictionary<LaserCell.Laser, GameObject> CreateLasersForLevel(Level level) {
            var lasers = new Dictionary<LaserCell.Laser, GameObject>();
            foreach (var line    in level.GetLaserLines()) {
                var laserObject = CreateLaserLine(line, level);
                lasers.Add(line, laserObject);
            }
            return lasers;
        }      
        
        private GameObject CreateLaserLine(LaserCell.Laser line, Level level) {
            var cover = line.GetCover();
            var coord = GameUtils.ConvertToGameCoord(cover.GetMinX(), cover.GetMinY(), level);
            var laserObject = GameUtils.InstantiateChild(laserLine, coord, laserLineFolder);

            // TODO Move this logic to LaserLine controller
            var lenght = GetLenght(line);
            ScaleLaser(laserObject, lenght);
            MoveLaser(laserObject, line, lenght);
            RotateLaser(laserObject, line);
            laserObject.SetActive(line.IsDanger());   
            return laserObject;
        }

        private float GetLenght(LaserCell.Laser line) {
            var cover = line.GetCover();
            if (line.IsVertical()) {
                return cover.GetMaxY() - cover.GetMinY() + 1;
            }
            return cover.GetMaxX() - cover.GetMinX() + 1;
        }

        private float GetSafeShift(float lenght) {
            return lenght == 1 ? 0 : (lenght - 1)/2;
        }

        private void ScaleLaser(GameObject laser, float lenght) {
            var localScaleX = laser.transform.localScale.x;
            laser.transform.localScale = new Vector3(lenght*localScaleX, 1, 1); 
        }

        private void MoveLaser(GameObject laser, LaserCell.Laser line, float lenght) {
            laser.transform.position += new Vector3(
                    line.IsHorizontal() ? GetSafeShift(lenght) : 0,
                    line.IsVertical() ? GetSafeShift(lenght) : 0,
                    0);
        }

        private void RotateLaser(GameObject laser, LaserCell.Laser line) {
            if (line.IsVertical()) {
                laser.transform.Rotate(0, 0, 90);
            }
        }
    }

}