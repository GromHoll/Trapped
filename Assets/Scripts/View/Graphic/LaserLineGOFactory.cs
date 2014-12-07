using System.Collections.Generic;
using System.Xml;
using TrappedGame.Model;
using TrappedGame.Model.Cells;
using TrappedGame.Utils;
using TrappedGame.View.Controllers;
using UnityEngine;

namespace TrappedGame.View.Graphic {
    public class LaserLineGOFactory : MonoBehaviour {

        public GameObject laserLineFolder;
        public GameObject laserLine;

        public void CreateLasersForLevel(Level level) {
            foreach (Cell cell in level.GetCells()) {
                if (cell.GetCellType() == CellType.LASER) {
                    var laser = (LaserCell) cell;
                    foreach (var line in laser.GetLaserLines()) {
                        CreateLaserLine(line, level);
                    }
                }
            }
        }      
        
        private void CreateLaserLine(LaserCell.Laser line, Level level) {
            var cover = line.GetCover();
            var coord = GameUtils.ConvertToGameCoord(cover.GetMinX(), cover.GetMinY(), level);
            var laserObject = GameUtils.InstantiateChild(laserLine, coord, laserLineFolder);
            var controller = laserObject.GetComponent<LaserLineController>();
            controller.SetLaserLine(line);
        }
    }
}