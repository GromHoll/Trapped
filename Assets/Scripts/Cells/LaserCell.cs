using UnityEngine;

public class LaserCell : CountCell {
    
    protected bool up;
    protected bool right;
    protected bool down;
    protected bool left;

    public LaserCell(int x, int y, 
                     int onPeriod, int offPeriod, int currentTick, bool isOn,
                     bool up, bool right, bool down, bool left) 
        : base(x, y, CellType.LASER, onPeriod, offPeriod, currentTick, isOn) {
        this.up = up;
        this.right = right;
        this.down = down;
        this.left = left;
    }

    public LaserCell(int x, int y, bool up, bool right, bool down, bool left)
        : base(x, y, CellType.LASER) {
        this.up = up;
        this.right = right;
        this.down = down;
        this.left = left;
    }

    public override bool IsBocked() {
        return true;
    }

    public bool IsUp() { 
        return up;
    }
    
    public bool IsRight() { 
        return right;
    }

    public bool IsDown() { 
        return down;
    }

    public bool IsLeft() { 
        return left;
    }
}
