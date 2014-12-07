using TrappedGame.Model.Cells;
using TrappedGame.Model.Common;

namespace TrappedGame.Model.LevelUtils {
    public class LaserHelper {

        public LaserCell.Laser CreateUpLaser(LaserCell laser, Level level) {
            if(!laser.IsUp()) return null;
            
            var lenght = 0;
            for (var y = laser.Y + 1; y < level.SizeY; y++, lenght++) {
                if (level.GetCell(laser.X, y).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Laser(laser, new IntRect(laser.X, laser.Y + 1, laser.X, laser.Y + lenght))
                : null;
        }

        public LaserCell.Laser CreateDownLaser(LaserCell laser, Level level) {
            if(!laser.IsDown()) return null;
            
            var lenght = 0;
            for (var y = laser.Y - 1; y >= 0; y--, lenght++) {
                if (level.GetCell(laser.X, y).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Laser(laser, new IntRect(laser.X, laser.Y - 1, laser.X, laser.Y - lenght))
                : null;
        }

        public LaserCell.Laser CreateRightLaser(LaserCell laser, Level level) {
            if(!laser.IsRight()) return null;

            var lenght = 0;
            for (var x = laser.X + 1; x < level.SizeX; x++, lenght++) {
                if (level.GetCell(x, laser.Y).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Laser(laser,new IntRect(laser.X + 1, laser.Y, laser.X + lenght, laser.Y))
                : null;
        }

        public LaserCell.Laser CreateLeftLaser(LaserCell laser, Level level) {
            if(!laser.IsLeft()) return null;
            
            var lenght = 0;
            for (var x = laser.X - 1; x >= 0; x--, lenght++) {
                if (level.GetCell(x, laser.Y).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Laser(laser,new IntRect(laser.X - 1, laser.Y, laser.X - lenght, laser.Y))
                : null;   
        }
    }
}