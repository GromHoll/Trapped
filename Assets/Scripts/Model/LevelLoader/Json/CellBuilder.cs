using SimpleJSON;
using UnityEngine;

using TrappedGame.Model.LevelUtils;
using TrappedGame.Model.Common;
using TrappedGame.Model.Cells;

namespace TrappedGame.Model.LevelLoader.Json {
	public class CellBuilder : DefaultCellBuilder {

        // TODO Delete class field
        private JSONNode cellDescription;

		public override void MakeCell(JSONNode description, LevelBuilder builder, IntVector2 coordinate) {
			cellDescription = description;
			var cellType = cellDescription["type"].Value;
            switch (cellType) {
                case "LASER"    : MakeLaser(builder, coordinate);       break;
                case "SPEAR"    : MakeSpear(builder, coordinate);       break;
                case "TIME"     : MakeTimeBonus(builder, coordinate);   break;
                case "EMPTY"    : MakeEmpty(builder, coordinate);       break;
                case "WALL"     : MakeWall(builder, coordinate);        break;
                case "START"    : MakeStart(builder, coordinate);       break;
                case "FINISH"   : MakeFinish(builder, coordinate);      break;
                case "BONUS"    : MakeBonus(builder, coordinate);       break;
                default         : Debug.Log("Unknown cell type");       break;
            }
		}

		protected override void MakeLaser(LevelBuilder builder, IntVector2 coordinate) {
			int onPeriod, offPeriod, currentPeriod;
			bool isOn;
			
			ReadPeriodInfo(out onPeriod, out offPeriod, out currentPeriod, out isOn);

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
			int onPeriod, offPeriod, currentPeriod;
			bool isOn;
			
			ReadPeriodInfo(out onPeriod, out offPeriod, out currentPeriod, out isOn);

			builder.AddCell(new SpearCell(coordinate.x, coordinate.y, onPeriod, offPeriod, currentPeriod, isOn));
		}

		protected override void MakeTimeBonus(LevelBuilder builder, IntVector2 coordinate) {
			int levelTickNum = cellDescription["tick"].AsInt;

			builder.AddCell(new EmptyCell(coordinate.x, coordinate.y));
			builder.AddTimeBonus(coordinate, new LevelTick(levelTickNum));
		}

		private void ReadPeriodInfo(out int onPeriod, out int offPeriod, out int currentPeriod, out bool isOn) {
			onPeriod = cellDescription["onPeriod"].AsInt;
			offPeriod = cellDescription["offPeriod"].AsInt;
			currentPeriod = cellDescription["currentPeriod"].AsInt;
			
			isOn = (cellDescription["currentState"].Value == "on" ? true : false);
		}
        
	}
}
