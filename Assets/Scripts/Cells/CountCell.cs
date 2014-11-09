using UnityEngine;
using System.Collections;

public class CountCell : Cell {
    public bool isOn;
    public int onPeriod;
    public int offPeriod;

    public int currentTick;

    public CountCell(
        Vector2 coordinate, CellType type, int onPeriod, int offPeriod, int currentTick, bool isOn)
            : base(coordinate, type) {

        this.type = type;
        this.onPeriod = onPeriod;
        this.offPeriod = offPeriod;
        this.currentTick = currentTick;
        this.isOn = isOn;
    }

    public CountCell(Vector2 coordinate, CellType type)
        : this(coordinate, type, 1, 1, 0, false) {}

    public override void nextTick() {
        currentTick++;
        if (currentTick == getCurrentPeriod()) {
            currentTick = 0;
            isOn = !isOn;
        }
    }

    public override void backTick() {
        currentTick--;
        if (currentTick == -1) {
            currentTick = getCurrentPeriod() - 1;
            isOn = !isOn;
        }
    }

    private int getCurrentPeriod() {
        return isOn ? onPeriod : offPeriod;
    }
}
