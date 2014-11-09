using UnityEngine;
using System.Collections;

public class LaserCell : CountCell {
    
    public bool up;
    public bool right;
    public bool down;
    public bool left;

    public LaserCell(
        bool up, bool right, bool down, bool left, Vector2 coordinate, 
        int onPeriod, int offPeriod, int currentTick, bool isOn
    ) : base(coordinate, CellType.LASER, onPeriod, offPeriod, currentTick, isOn) {
        this.up = up;
        this.right = right;
        this.down = down;
        this.left = left;
    }

	public LaserCell(bool up, bool right, bool down, bool left, Vector2 coordinate)
        : base(coordinate, CellType.LASER) {
		this.up = up;
		this.right = right;
		this.down = down;
		this.left = left;
	}
}
