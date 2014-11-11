using System;
using UnityEngine;
using System.Collections.Generic;

public class Level : ITickable {
    
    private string name;
    
    private IntVector2 size;
    private Cell[,] cells;

    private IntVector2 start;
    private IntVector2 finish;
    private IList<IntVector2> bonuses = new List<IntVector2>();

    public Level(string name, int xSize, int ySize) {
        if (xSize <= 0) throw new ArgumentException("Size should be positive", "xSize");
        if (ySize <= 0) throw new ArgumentException("Size should be positive", "ySize");

        size = new IntVector2(xSize, ySize);
        cells = new Cell[size.x, size.y];
        for (int x = 0; x < size.x; x++) {
            for (int y = 0; y < size.y; y++) {
                AddCell(new Cell(new Vector2(x, y), CellType.UNKNOWN));
            }
        }
    }

    public void AddBonus(IntVector2 coordinate) {
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

    public int GetStartX() {
        return start.x;
    }

    public int GetStartY() {
        return start.y;
    }

    public void SetStart(int x, int y) {
        start.Set(x, y);
    }

    public int GetFinishX() {
        return finish.x;
    }
    
    public int GetFinishY() {
        return finish.y;
    }

    public void SetFinish(int x, int y) {
        finish.Set(x, y);
    }

    public int GetSizeX() {
        return size.x;
    }
    
    public int GetSizeY() {
        return size.y;
    }

    public Cell GetCell(int x, int y) {
        if (x < 0 || x >= size.x) throw new ArgumentException("Size should be positive and less than xSize", "x");
        if (y < 0 || y >= size.y) throw new ArgumentException("Size should be positive and less than ySize", "y");
        return cells[x, y];
    }

    public IList<IntVector2> GetBonuses() {
        return bonuses;
    }

    // TODO Remove convector methods after refactor
    public Vector2 ConvertToGameCoord(Vector2 pos) {
        return ConvertToGameCoord(pos.x, pos.y);
    }

    public Vector2 ConvertToGameCoord(IntVector2 pos) {
        return ConvertToGameCoord(pos.x, pos.y);
    }

    public Vector2 ConvertToGameCoord(float x, float y) {
        float gameX = x - (size.x - 1)/2f;
        float gameY = y - (size.y - 1)/2f;
        return new Vector2(gameX, gameY);
    }
    
    public Vector2 ConvertToLevelCoord(Vector2 pos) {
        float levelX = pos.x + (size.x - 1)/2f;
        float levelY = pos.y + (size.y - 1)/2f;
        return new Vector2(levelX, levelY);
    }
}
