using UnityEngine;

public class SpearCell : CountCell {

    public SpearCell(Vector2 coordinate, int onPeriod, int offPeriod, int currentTick, bool isOn)
        : base(coordinate, CellType.SPEAR, onPeriod, offPeriod, currentTick, isOn) {}
    
    public SpearCell(Vector2 coordinate)
        : base(coordinate, CellType.SPEAR) {}


    public override bool IsDeadly() {
        return isOn;
    }
}

