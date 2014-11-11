using UnityEngine;

public class WallCell : Cell {
 
    public WallCell(int x, int y) : base(x, y, CellType.WALL) {}

    public override bool IsBocked() {
        return true;
    }
}