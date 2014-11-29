using TrappedGame.Model.Cells;
using TrappedGame.Model.Common;

namespace TrappedGame.Model.LevelUtils {
    public class LaserHelper {

        public LaserCell.Laser CreateUpLaser(LaserCell laser, Level level) {
            if(!laser.IsUp()) return null;
            
            var laserX = laser.GetX();
            var laserY = laser.GetY();
            var lenght = 0;
            for (var y = laserY + 1; y < level.GetSizeY(); y++, lenght++) {
                if (level.GetCell(laserX, y).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Laser(laser, new IntRect(laserX, laserY + 1, laserX, laserY + lenght))
                : null;
        }

        public LaserCell.Laser CreateDownLaser(LaserCell laser, Level level) {
            if(!laser.IsDown()) return null;
            
            var laserX = laser.GetX();
            var laserY = laser.GetY();
            var lenght = 0;
            for (var y = laserY - 1; y >= 0; y--, lenght++) {
                if (level.GetCell(laserX, y).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Laser(laser, new IntRect(laserX, laserY - 1, laserX, laserY - lenght))
                : null;
        }

        public LaserCell.Laser CreateRightLaser(LaserCell laser, Level level) {
            if(!laser.IsRight()) return null;

            var laserX = laser.GetX();
            var laserY = laser.GetY();
            var lenght = 0;
            for (var x = laserX + 1; x < level.GetSizeX(); x++, lenght++) {
                if (level.GetCell(x, laserY).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Laser(laser,new IntRect(laserX + 1, laserY, laserX + lenght, laserY))
                : null;
        }

        public LaserCell.Laser CreateLeftLaser(LaserCell laser, Level level) {
            if(!laser.IsLeft()) return null;
            
            var laserX = laser.GetX();
            var laserY = laser.GetY();
            var lenght = 0;
            for (var x = laserX - 1; x >= 0; x--, lenght++) {
                if (level.GetCell(x, laserY).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Laser(laser,new IntRect(laserX - 1, laserY, laserX - lenght, laserY))
                : null;   
        }
    }
}