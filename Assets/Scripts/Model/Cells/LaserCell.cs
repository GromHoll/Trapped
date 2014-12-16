using System.Collections.Generic;
using System.Linq;
using TrappedGame.Model.Common;
using TrappedGame.Model.LevelUtils;

namespace TrappedGame.Model.Cells {
    public class LaserCell : CountCell {

        public class Line {
            public LaserCell Owner { get; protected set; }
            public IntRect Cover { get; protected set; }

            public Line(LaserCell owner, IntRect line) {
                Owner = owner;
                Cover = line;
            }

            public bool IsDangerFor(int x, int y) {
                return Cover.Contains(x, y) && IsDanger();
            }
            
            public bool IsDanger() {
                return Owner.IsDeadly();
            }

            public bool IsHorizontal() {
                return Cover.MaxY == Cover.MinY && Cover.MaxY == Owner.Y;
            }

            public bool IsVertical() {
                return Cover.MaxX == Cover.MinX && Cover.MaxX == Owner.X;
            }
        }

        public bool Up    { get; protected set; }
        public bool Right { get; protected set; }
        public bool Down  { get; protected set; }
        public bool Left  { get; protected set; }
        public IList<Line> LaserLines { get; protected set; }

        public LaserCell(int x, int y, 
                         int onPeriod, int offPeriod, int currentTick, bool isOn,
                         bool up, bool right, bool down, bool left) 
            : base(x, y, onPeriod, offPeriod, currentTick, isOn) {
            Up = up;
            Right = right;
            Down = down;
            Left = left;
        }

        public LaserCell(int x, int y, bool up, bool right, bool down, bool left) : base(x, y) {
            Up = up;
            Right = right;
            Down = down;
            Left = left;
        }

        public override bool IsBlocked() {
            return true;
        }

        public override bool IsDeadly() {
            return IsOn;
        }

        public override bool IsDeadlyFor(int x, int y) {
            return LaserLines.Any(laser => laser.IsDangerFor(x, y));
        }
        
        public void CreateLaserLines(LaserHelper helper, Level level) {
            LaserLines = helper.CreateLaserLines(this, level);
        }
    }
}
