using UnityEngine;
using System.Collections;


public class Level {

	public Cell[,] cells;
	public int xSize;
	public int ySize;

	public Vector2 start;
	public Vector2 finish;

	public IList bonuses;

	public string name;

	public Level(int xSize, int ySize, string name) {
		this.xSize = xSize;
		this.ySize = ySize;

		this.cells = new Cell[xSize, ySize];
		for (int x = 0; x < xSize; x++) {
			for (int y = 0; y < ySize; y++) {
				addCell(new Tile(new Vector2(x, y), CellType.UNKNOWN));
			}
		}

		bonuses = new ArrayList();
	}

	public void addBonus(Vector2 coordinate) {
		bonuses.Add(coordinate);
	}

	public void addCell(Cell cell) {
        int x = cell.getX();
        int y = cell.getY();
		cells[x, y] = cell;
	}

	public void nextTick() {
        foreach (Cell cell in cells) {
            cell.nextTick();
        }
    }

	public void backTick() {
		//foreach (Cell[] row in cells) {
		foreach (Cell cell in cells) {
				cell.backTick();		
			}
		//}
	}
}
