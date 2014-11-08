using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;



public class Loader {
	public Loader(string levelFile) {
		description = new Dictionary<char, string>();

		StreamReader reader = File.OpenText(levelFile.ToString());
		string line;

		// Read Level Name
		levelName = reader.ReadLine();
		line = reader.ReadLine();

		// Read Level Size
		string[] items = line.Split(' ');
		xSize = int.Parse(items[0]);
		ySize = int.Parse(items[1]);

		symbols = new char[ySize, xSize];
		for (int y = 0; y < ySize; y++) {
			line = reader.ReadLine();
			for(int x = 0; x < xSize; x++) {
				symbols[y, x] = line[x];
			}
		}

		// Read Descriptions
		int descriptionCounter;

		line = reader.ReadLine();
		descriptionCounter = int.Parse(line);

		for (int i = 0; i < descriptionCounter; i++) {
			line = reader.ReadLine ();
			string[] descript = line.Split (':');

			description.Add (descript [0] [0], descript [1]);
		}
	}

	public Level makeLevel() {
		Level l = new Level (xSize, ySize, levelName);

		Cell cell;
		for (int y = 0; y < ySize; y++) {
			for(int x = 0; x < xSize; x++) {
				char currentSymbol = symbols[y, x];
				if(currentSymbol=='s') {
					l.start = new Vector2(x, y);
				} else if(currentSymbol=='f') {
					l.finish = new Vector2(x, y);
				} else if(currentSymbol=='b') {
					l.addBonus(new Vector2(x, y));
				}

				if(description.Contains(symbols[y, x])) {
					string d = (string)description[symbols[y, x]];
					cell = CellFactory.getCellByDescription(d, new Vector2(x, y));
				} else {
					cell = CellFactory.getCellBySymbol(symbols[y, x], new Vector2(x, y));
				}

				l.addCell(cell);
			}
		}

		return l;
	}

	int xSize, ySize;
	string levelName;
	char[,] symbols;
	IDictionary description;
}
