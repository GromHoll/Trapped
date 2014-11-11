using System;
using UnityEngine;


public class Hero {

    private IntVector2 position = new IntVector2(0, 0);

    public Hero(int x, int y) {
        MoveTo(x, y);
    }

    public int GetX() {
        return position.x;
    }

    public int GetY() {
        return position.y;
    }

    public void MoveTo(int x, int y) {
        position.x = x;
        position.y = y;
    }
}