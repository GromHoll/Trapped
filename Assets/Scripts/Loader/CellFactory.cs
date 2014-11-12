using UnityEngine;
using System.Collections;

public class CellFactory {

    public Cell getCellByDescription(string description, int x, int y) {
        Cell result;
        bool isOn = false;
        int onPeriod = 1;
        int offPeriod = 1;
        int currentTick = 0;

        string[] items = description.Split (' ');
        string name = items [0];

        if (name == "laser") {
            bool up = false, right = false, down = false, left = false;
            result = new LaserCell(x, y, false, false, false, false);
            //ud off 3 3 1
            int directionCounter = items [1].Length;
            for (int i = 0; i< directionCounter; i++) {
            switch (items [1] [i]) {
                case 'u': up = true; break;
                case 'r': right = true; break;
                case 'd': down = true; break;
                case 'l': left = true; break;
                }
            }

            if (items [2] == "on") { isOn = true; }

            onPeriod = int.Parse (items [3]);
            offPeriod = int.Parse (items [4]);
            currentTick = int.Parse (items [5]);

            result = new LaserCell (x, y, onPeriod, offPeriod, currentTick, isOn, up, right, down, left);

        } else if (name == "spear") {
            if (items [1] == "on") { isOn = true; }

            onPeriod = int.Parse (items [2]);
            offPeriod = int.Parse (items [3]);
            currentTick = int.Parse (items [4]);

            result = new SpearCell(x, y, onPeriod, offPeriod, currentTick, isOn);
        } else {
            return new Cell(x, y, CellType.UNKNOWN);
        }

        return result;
    }

    public Cell getCellBySymbol(char symbol, int x, int y) {
        switch (symbol) {
            case '.' : return new Cell(x, y);
            case '#' : return new WallCell(x, y);
            case 'S' : return new SpearCell(x, y);
            case 'L' : return new LaserCell(x, y, true, true, true, true);
            case 's' : return new Cell(x, y);
            case 'f' : return new Cell(x, y);
            case 'b' : return new Cell(x, y);
        }
        return new Cell(x, y, CellType.UNKNOWN);
    }
}
