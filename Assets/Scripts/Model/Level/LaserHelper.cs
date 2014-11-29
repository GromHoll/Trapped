using System;
namespace TrappedGame {

    public class LaserHelper {

        public LaserCell.Laser CreateUpLaser(LaserCell laser, Level level) {
            if(!laser.IsUp()) return null;
            
            int laserX = laser.GetX();
            int laserY = laser.GetY();
            int lenght = 0;
            for (int y = laserY + 1; y < level.GetSizeY(); y++, lenght++) {
                if (level.GetCell(laserX, y).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Laser(laser, new IntRect(laserX, laserY + 1, laserX, laserY + lenght))
                : null;
        }

        public LaserCell.Laser CreateDownLaser(LaserCell laser, Level level) {
            if(!laser.IsDown()) return null;
            
            int laserX = laser.GetX();
            int laserY = laser.GetY();
            int lenght = 0;
            for (int y = laserY - 1; y >= 0; y--, lenght++) {
                if (level.GetCell(laserX, y).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Laser(laser, new IntRect(laserX, laserY - 1, laserX, laserY - lenght))
                : null;
        }

        public LaserCell.Laser CreateRightLaser(LaserCell laser, Level level) {
            if(!laser.IsRight()) return null;

            int laserX = laser.GetX();
            int laserY = laser.GetY();
            int lenght = 0;
            for (int x = laserX + 1; x < level.GetSizeX(); x++, lenght++) {
                if (level.GetCell(x, laserY).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Laser(laser,new IntRect(laserX + 1, laserY, laserX + lenght, laserY))
                : null;
        }

        public LaserCell.Laser CreateLeftLaser(LaserCell laser, Level level) {
            if(!laser.IsLeft()) return null;
            
            int laserX = laser.GetX();
            int laserY = laser.GetY();
            int lenght = 0;
            for (int x = laserX - 1; x >= 0; x--, lenght++) {
                if (level.GetCell(x, laserY).IsBocked()) { break; }
            }
            return (lenght != 0)
                ? new LaserCell.Laser(laser,new IntRect(laserX - 1, laserY, laserX - lenght, laserY))
                : null;   
        }
    }
}

