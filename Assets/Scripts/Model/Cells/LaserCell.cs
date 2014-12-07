using System.Collections.Generic;
using System.Linq;
using TrappedGame.Model.Common;
using TrappedGame.Model.LevelUtils;

namespace TrappedGame.Model.Cells {
    public class LaserCell : CountCell {

        public class Laser {
            private readonly LaserCell owner;
            private readonly IntRect line;

            public Laser(LaserCell owner, IntRect line) {
                this.owner = owner;
                this.line = line;
            }

            public bool IsDangerFor(int x, int y) {
                return line.Contains(x, y) && IsDanger();
            }
            
            public bool IsDanger() {
                return owner.IsDeadly();
            }

            public IntRect GetCover() {
                return line;
            }

            public bool IsHorizontal() {
                return line.GetMaxY() == line.GetMinY() && line.GetMaxY() == owner.Y;
            }

            public bool IsVertical() {
                return line.GetMaxX() == line.GetMinX() && line.GetMaxX() == owner.X;
            }
        }

        private readonly bool up;
        private readonly bool right;
        private readonly bool down;
        private readonly bool left;

        private readonly IList<Laser> laserLines = new List<Laser>();

        public LaserCell(int x, int y, 
                         int onPeriod, int offPeriod, int currentTick, bool isOn,
                         bool up, bool right, bool down, bool left) 
            : base(x, y, CellType.LASER, onPeriod, offPeriod, currentTick, isOn) {
            this.up = up;
            this.right = right;
            this.down = down;
            this.left = left;
        }

        public LaserCell(int x, int y, bool up, bool right, bool down, bool left)
            : base(x, y, CellType.LASER) {
            this.up = up;
            this.right = right;
            this.down = down;
            this.left = left;
        }

        public override bool IsBocked() {
            return true;
        }

        public override bool IsDeadly() {
            return IsOn;
        }

        public override bool IsDeadlyFor(int x, int y) {
            return laserLines.Any(laser => laser.IsDangerFor(x, y)); ;
        }

        public bool IsUp() { 
            return up;
        }
        
        public bool IsRight() { 
            return right;
        }

        public bool IsDown() { 
            return down;
        }

        public bool IsLeft() { 
            return left;
        }

        public IList<Laser> GetLaserLines() {
            return laserLines;
        }

        private void AddLaserLine(Laser line)  {
            if (line != null) {
                laserLines.Add(line);  
            }  
        }

        public void CreateLaserLines(LaserHelper helper, Level level) {
            // TODO move all 4 calls to helper
            AddLaserLine(helper.CreateUpLaser(this, level));
            AddLaserLine(helper.CreateRightLaser(this, level));
            AddLaserLine(helper.CreateDownLaser(this, level));
            AddLaserLine(helper.CreateLeftLaser(this, level));    
        }

    }
}
