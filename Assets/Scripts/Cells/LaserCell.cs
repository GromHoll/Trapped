﻿using UnityEngine;
using TrappedGame;
// TODO add to namespace

public class LaserCell : CountCell {

    public class Laser {
        private LaserCell owner;
        private IntRect line;

        public Laser(LaserCell owner, IntRect line) {
            this.owner = owner;
            this.line = line;
        }

        public bool IsDangerFor(int x, int y) {
            return line.Contains(x, y) && owner.IsDeadly();
        }

        public IntRect GetCover() {
            return line;
        }
    }
    
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

    public override bool IsDeadly() {
        return isOn;
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
