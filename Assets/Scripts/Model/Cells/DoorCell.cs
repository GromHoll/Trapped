using System.Collections.Generic;
using System.Linq;
using TrappedGame.Model.Elements;
using TrappedGame.Utils;

namespace TrappedGame.Model.Cells {
    public class DoorCell : Cell {

        private readonly IList<Key> keys = new List<Key>();

        public DoorCell(int x, int y) : base(x, y) {}

        public override bool IsBlocked() {
            return keys.All(k => !k.PickedUp());
        }

        public void AddKey(Key key) {
            Validate.NotNull(key, "Key can't be null");
            keys.Add(key);
        }
    }
}
