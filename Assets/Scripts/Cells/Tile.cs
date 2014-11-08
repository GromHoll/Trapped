using System;
using UnityEngine;		

public class Tile : Cell {

	public Tile(Vector2 coordinate, CellType type) : base(coordinate, type)
	{	}

	public override void nextTick() { }
	public override void backTick() { }
}

