using System.Collections.Generic;
using System.Linq;
using TrappedGame.Model.Elements;
using TrappedGame.Utils;

namespace TrappedGame.Model.Cells {
    public class PitCell : Cell {

        private IList<Platform> platforms = new List<Platform>();

        public PitCell(int x, int y) : base(x, y) {}

        public override bool IsDeadly() {
            return platforms.All(platform => platform.Coordinate != Coordinate);
        }

        public void AddPlatforms(IList<Platform> newPlatforms) {
            Validate.NotNull(newPlatforms);
            platforms = newPlatforms;
        }

    }
}
