using UnityEngine;

public class Cell {

    protected Vector2 coordinate;
    protected CellType type;
   
    public Cell(Vector2 coordinate) {
        this.coordinate = coordinate;
        this.type = CellType.EMPTY;
    }

    public Cell(Vector2 coordinate, CellType type) {
        this.coordinate = coordinate;
        this.type = type;
    }

    public virtual bool IsBocked() {
        return false;
    }

    public virtual bool IsDeadly() {
        return false;
    }

    public Vector2 getCoordinate() {
        return coordinate;
    }

    public int getX() {
        return (int) coordinate.x;
    }

    public int getY() {
        return (int) coordinate.y;
    }
    
    public CellType getType() {
        return type;
    }

    public virtual void nextTick() {}
    public virtual void backTick() {}
}
