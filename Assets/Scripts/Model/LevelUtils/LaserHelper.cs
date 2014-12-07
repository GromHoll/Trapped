using System.Collections.Generic;
using TrappedGame.Model.Cells;
using TrappedGame.Model.Common;

namespace TrappedGame.Model.LevelUtils {
    public class LaserHelper {

        public LaserCell.Line CreateUpLaser(LaserCell laser, Level level) {
            if(!laser.Up) return null;
            
            var lenght = 0;
            for (var y = laser.Y + 1; y < level.SizeY; y++, lenght++) {
                if (level.GetCell(laser.X, y).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Line(laser, new IntRect(laser.X, laser.Y + 1, laser.X, laser.Y + lenght))
                : null;
        }

        public LaserCell.Line CreateDownLaser(LaserCell laser, Level level) {
            if(!laser.Down) return null;
            
            var lenght = 0;
            for (var y = laser.Y - 1; y >= 0; y--, lenght++) {
                if (level.GetCell(laser.X, y).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Line(laser, new IntRect(laser.X, laser.Y - 1, laser.X, laser.Y - lenght))
                : null;
        }

        public LaserCell.Line CreateRightLaser(LaserCell laser, Level level) {
            if(!laser.Right) return null;

            var lenght = 0;
            for (var x = laser.X + 1; x < level.SizeX; x++, lenght++) {
                if (level.GetCell(x, laser.Y).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Line(laser,new IntRect(laser.X + 1, laser.Y, laser.X + lenght, laser.Y))
                : null;
        }

        public LaserCell.Line CreateLeftLaser(LaserCell laser, Level level) {
            if(!laser.Left) return null;
            
            var lenght = 0;
            for (var x = laser.X - 1; x >= 0; x--, lenght++) {
                if (level.GetCell(x, laser.Y).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Line(laser,new IntRect(laser.X - 1, laser.Y, laser.X - lenght, laser.Y))
                : null;   
        }

        public IList<LaserCell.Line> CreateLaserLines(LaserCell laser, Level level) {
            IList<LaserCell.Line> result = new List<LaserCell.Line>();
            AddIfNotNull(result, CreateUpLaser(laser, level));
            AddIfNotNull(result, CreateRightLaser(laser, level));
            AddIfNotNull(result, CreateDownLaser(laser, level));
            AddIfNotNull(result, CreateLeftLaser(laser, level));
            return result;
        }
        
        // TODO Move to utils
        private void AddIfNotNull<T>(IList<T> list, T element) {
            if (element != null) {
                list.Add(element);   
            }
        }


    }
}