using UnityEngine;

public class CountCell : Cell {

    public static readonly int DEFAULT_PERIOD = 1;
    public static readonly bool DEFAULT_ON_STATE = false;

    protected bool isOn;
    protected int onPeriod;
    protected int offPeriod;
    protected int currentTick;

    public CountCell(Vector2 coordinate, CellType type, int onPeriod, int offPeriod, int currentTick, bool isOn)
        : base(coordinate, type) {
        this.onPeriod = onPeriod;
        this.offPeriod = offPeriod;
        this.currentTick = currentTick;
        this.isOn = isOn;
    }

    public CountCell(Vector2 coordinate, CellType type)
        : this(coordinate, type, DEFAULT_PERIOD, DEFAULT_PERIOD, 0, DEFAULT_ON_STATE) {}

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

    public bool IsOn() {
        return isOn;
    }

    private int getCurrentPeriod() {
        return isOn ? onPeriod : offPeriod;
    }
}
