using TrappedGame.Model.Common;

namespace TrappedGame.Model.Elements {
	public class Key {

        // TODO Create map element parent
        protected IntVector2 position;
        public IntVector2 Coordinate { get { return position; } }
        public int X { get { return position.x; } }
        public int Y { get { return position.y; } }

	    private bool isPickedUp;

	    public Key(int x, int y) {
	        position = new IntVector2(x, y);        
	    }

	    public void PickUp() {
	        isPickedUp = true;
	    }

        public void Drop() {
            isPickedUp = false;
	        
	    }

        public bool PickedUp() {
            return isPickedUp;
        }

	}
}
