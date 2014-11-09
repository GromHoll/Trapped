using UnityEngine;

public class WallCell : Cell {
 
    public WallCell(Vector2 coordinate) : base(coordinate, CellType.WALL) {}

    public override bool IsBocked() {
        return true;
    }
}