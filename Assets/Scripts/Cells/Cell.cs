using UnityEngine;

public class Cell : ITickable {

    protected Vector2 coordinate;
    protected CellType cellType;
   
    public Cell(Vector2 coordinate) {
        this.coordinate = coordinate;
        this.cellType = CellType.EMPTY;
    }

    public Cell(Vector2 coordinate, CellType type) {
        this.coordinate = coordinate;
        this.cellType = type;
    }

    public virtual bool IsBocked() {
        return false;
    }

    public virtual bool IsDeadly() {
        return false;
    }

    public Vector2 GetCoordinate() {
        return coordinate;
    }

    public int GetX() {
        return (int) coordinate.x;
    }

    public int GetY() {
        return (int) coordinate.y;
    }
    
    public CellType GetCellType() {
        return cellType;
    }

    public virtual void NextTick() {}
    public virtual void BackTick() {}
}
