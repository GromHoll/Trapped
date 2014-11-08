using UnityEngine;
using System.Collections;


public abstract class Cell {
	public Vector2 coordinate;
	public CellType type;

	public bool IsBocked() {
		return type == CellType.LASER || type == CellType.WALL;
							
	}

	public Cell(Vector2 coordinate, CellType type = CellType.UNKNOWN) {
		this.coordinate = coordinate;
		this.type = type;
	}

	public abstract void nextTick();
	public abstract void backTick();
}
