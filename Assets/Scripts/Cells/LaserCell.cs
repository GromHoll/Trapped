using UnityEngine;
using System.Collections;

public class LaserCell : CountCell {
	public LaserCell(
		bool up, bool right, bool down, bool left, Vector2 coordinate,
		int onPeriod = 1, int offPeriod = 1, int currentTick = 0, bool isOn = false
	) : base(coordinate, CellType.LASER, onPeriod, offPeriod, currentTick, isOn) {
		this.up = up;
		this.right = right;
		this.down = down;
		this.left = left;
	}

	public bool up;
	public bool right;
	public bool down;
	public bool left;
}
