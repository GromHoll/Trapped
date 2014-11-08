using UnityEngine;
using System.Collections;

public class CellFactory {
	public static Cell getCellByDescription(string description, Vector2 coordinate) {
		Cell result;
		bool isOn = false;
		int onPeriod = 1;
		int offPeriod = 1;
		int currentTick = 0;

		string[] items = description.Split (' ');
		string name = items [0];

		if (name == "laser") {
			bool up = false, right = false, down = false, left = false;
			result = new LaserCell (false, false, false, false, coordinate);
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

			result = new LaserCell (up, right, down, left, coordinate, onPeriod, offPeriod, currentTick, isOn);

		} else if (name == "spear") {
			if (items [1] == "on") { isOn = true; }

			onPeriod = int.Parse (items [2]);
			offPeriod = int.Parse (items [3]);
			currentTick = int.Parse (items [4]);

			result = new CountCell (coordinate, CellType.SPEAR, onPeriod, offPeriod, currentTick, isOn);
		} else {
			return new Tile (coordinate, CellType.UNKNOWN);
		}

		return result;
	}

	public static Cell getCellBySymbol(char symbol, Vector2 coordinate) {
		switch (symbol) {
			case '.' : return new Tile(coordinate, CellType.EMPTY);
			case '#' : return new Tile(coordinate, CellType.WALL);
			case 'S' : return new CountCell(coordinate, CellType.SPEAR);
			case 'L' : return new LaserCell(true, true, true, true, coordinate);
			case 's' : return new Tile(coordinate, CellType.EMPTY);
			case 'f' : return new Tile(coordinate, CellType.EMPTY);
			case 'b' : return new Tile(coordinate, CellType.EMPTY);
		}
		return new Tile(coordinate, CellType.UNKNOWN);
	}
}
