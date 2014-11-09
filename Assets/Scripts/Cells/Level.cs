using UnityEngine;
using System.Collections;

public class Level {
    
    public string name;
    
    public int xSize;
    public int ySize;
    public Cell[,] cells;

    public Vector2 start;
    public Vector2 finish;
    public IList bonuses = new ArrayList();

    public Level(string name, int xSize, int ySize) {
        this.xSize = xSize;
        this.ySize = ySize;

        this.cells = new Cell[xSize, ySize];
        for (int x = 0; x < xSize; x++) {
            for (int y = 0; y < ySize; y++) {
                AddCell(new Cell(new Vector2(x, y), CellType.UNKNOWN));
            }
        }
    }

    public void AddBonus(Vector2 coordinate) {
        bonuses.Add(coordinate);
    }

    public void AddCell(Cell cell) {
        int x = cell.GetX();
        int y = cell.GetY();
        cells[x, y] = cell;
    }

    public void NextTick() {
        foreach (Cell cell in cells) {
            cell.NextTick();
        }
    }

    public void BackTick() {
        foreach (Cell cell in cells) {
            cell.BackTick();
        }
    }

    public Vector2 ConvertToGameCoord(Vector2 pos) {
        return ConvertToGameCoord(pos.x, pos.y);
    }

    public Vector2 ConvertToGameCoord(float x, float y) {
        float gameX = x - (xSize - 1)/2f;
        float gameY = y - (ySize - 1)/2f;
        return new Vector2(gameX, gameY);
    }
    
    public Vector2 ConvertToLevelCoord(Vector2 pos) {
        float levelX = pos.x + (xSize - 1)/2f;
        float levelY = pos.y + (ySize - 1)/2f;
        return new Vector2(levelX, levelY);
    }
}
