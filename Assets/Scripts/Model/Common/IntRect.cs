using System;

namespace TrappedGame.Model.Common {
    public class IntRect {

        private IntVector2 point1;
        private IntVector2 point2;

        public int MinX {
            get { return Math.Min(point1.x, point2.x); }
        }

        public int MaxX {
            get { return Math.Max(point1.x, point2.x); }
        }

        public int MinY {
            get { return Math.Min(point1.y, point2.y); }
        }

        public int MaxY {
            get { return Math.Max(point1.y, point2.y); }
        }

        public IntRect(int x1, int y1, int x2, int y2) {
            point1 = new IntVector2(x1, y1);
            point2 = new IntVector2(x2, y2);
        }

        public bool Contains(IntVector2 vec) {
            return Contains(vec.x, vec.y);
        }

        public bool Contains(int x, int y) {
            return Math.Min(point1.x, point2.x) <= x 
                && Math.Max(point1.x, point2.x) >= x
                && Math.Min(point1.y, point2.y) <= y 
                && Math.Max(point1.y, point2.y) >= y;
        }
    }
}