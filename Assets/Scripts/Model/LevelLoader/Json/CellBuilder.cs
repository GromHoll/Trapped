using UnityEngine;
using SimpleJSON;

using TrappedGame.Model.LevelUtils;
using TrappedGame.Model.Common;
using TrappedGame.Model.Cells;

namespace TrappedGame.Model.LevelLoader.Json {
	public class CellBuilder : DefaultCellBuilder {
		public override void MakeCell(JSONNode description, LevelBuilder builder, IntVector2 coordinate) {
			cellDescription = description;

			string cellType = cellDescription["type"].Value;

			if (cellType == "LASER") {
				MakeLaser (builder, coordinate); return;
			} else if (cellType == "SPEAR") {
				MakeSpear (builder, coordinate); return;
			} else if (cellType == "TIME") {
				MakeTimeBonus (builder, coordinate); return;
			} else if (cellType == "EMPTY") {
				MakeEmpty(builder, coordinate); return;
			} else if (cellType == "WALL") {
				MakeWall(builder, coordinate); return;
			} else if (cellType == "START") {
				MakeStart(builder, coordinate); return;
			} else if (cellType == "FINISH") {
				MakeFinish(builder, coordinate); return;
			} else if (cellType == "BONUS") {
				MakeBonus(builder, coordinate); return;
			} else {
				//TODO Not support type exception
			}
		}

		protected override void MakeLaser(LevelBuilder builder, IntVector2 coordinate) {
			int onPeriod = -1, offPeriod = -1, currentPeriod = -1;
			bool isOn = false;
			
			ReadPeriodInfo(onPeriod, offPeriod, currentPeriod, isOn);

			bool[] sides = new bool[4];
			int sideNum = 0;
			foreach(JSONNode side in cellDescription["dangerousSide"].AsArray) {
				sides[sideNum] = side.AsBool;
				sideNum++;
			}

			builder.AddCell(new LaserCell(coordinate.x, coordinate.y, onPeriod, offPeriod, currentPeriod,
			                              isOn, sides[0], sides[1], sides[2], sides[3])); 
		}

		protected override void MakeSpear(LevelBuilder builder, IntVector2 coordinate) {
			int onPeriod = -1, offPeriod = -1, currentPeriod = -1;
			bool isOn = false;

			ReadPeriodInfo(onPeriod, offPeriod, currentPeriod, isOn);

			builder.AddCell(new SpearCell(coordinate.x, coordinate.y, onPeriod, offPeriod, currentPeriod, isOn));
		}

		protected override void MakeTimeBonus(LevelBuilder builder, IntVector2 coordinate) {
			int levelTickNum = cellDescription["tick"].AsInt;

			builder.AddCell(new Cell(coordinate.x, coordinate.y, CellType.EMPTY));
			builder.AddTimeBonus(coordinate, new LevelTick(levelTickNum));
		}

		private void ReadPeriodInfo(int onPeriod, int offPeriod, int currentPeriod, bool isOn) {
			onPeriod = cellDescription["onPeriod"].AsInt;
			offPeriod = cellDescription["offPeriod"].AsInt;
			currentPeriod = cellDescription["currentPeriod"].AsInt;
			
			isOn = (cellDescription["currentState"].Value == "on" ? true : false);
		}

		private JSONNode cellDescription;

	}
}
