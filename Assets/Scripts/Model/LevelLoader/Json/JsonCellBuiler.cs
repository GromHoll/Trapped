using System;
using SimpleJSON;
using UnityEngine;

using TrappedGame.Model.LevelUtils;
using TrappedGame.Model.Common;
using TrappedGame.Model.Cells;

namespace TrappedGame.Model.LevelLoader.Json {
    // TODO Should be refactored
	public class JsonCellBuiler {

        // TODO Delete class field
        private JSONNode cellDescription;

		public void MakeCell(JSONNode description, LevelBuilder builder, IntVector2 coordinate) {
			cellDescription = description;
			var cellType = cellDescription["type"].Value;
            switch (cellType) {
                case "LASER"    : MakeLaser(builder, coordinate);       break;
                case "PIT"      : MakePit(builder, coordinate);         break;
                case "SPEAR"    : MakeSpear(builder, coordinate);       break;
                case "TIME"     : MakeTimeBonus(builder, coordinate);   break;
                case "EMPTY"    : MakeEmpty(builder, coordinate);       break;
                case "WALL"     : MakeWall(builder, coordinate);        break;
                case "START"    : MakeStart(builder, coordinate);       break;
                case "FINISH"   : MakeFinish(builder, coordinate);      break;
                case "BONUS"    : MakeBonus(builder, coordinate);       break;
                case "PLATFORM" : MakePlatform(builder, coordinate);    break;
                case "PORTAL"   : MakePortal(builder, coordinate);      break;
                case "DOOR"     : MakeDoor(builder, coordinate);        break;
                case "KEY"      : MakeKey(builder, coordinate);         break;
                default: Debug.Log(String.Format("Unknown cell type: {0}, Coordinate: {1}", cellType, coordinate));   break;
            }
		}

        private void MakeStart(LevelBuilder builder, IntVector2 coordinate) {
            builder.AddCell(new EmptyCell(coordinate.x, coordinate.y));
            builder.SetStart(coordinate.x, coordinate.y);
        }

        private void MakeFinish(LevelBuilder builder, IntVector2 coordinate) {
            builder.AddCell(new EmptyCell(coordinate.x, coordinate.y));
            builder.SetFinish(coordinate.x, coordinate.y);
        }

        private void MakeEmpty(LevelBuilder builder, IntVector2 coordinate) {
            builder.AddCell(new EmptyCell(coordinate.x, coordinate.y));
        }

        private void MakeWall(LevelBuilder builder, IntVector2 coordinate) {
            builder.AddCell(new WallCell(coordinate.x, coordinate.y));
        }

        private void MakePit(LevelBuilder builder, IntVector2 coordinate) {
            builder.AddCell(new PitCell(coordinate.x, coordinate.y));
        }

        private void MakePlatform(LevelBuilder builder, IntVector2 coordinate) {
            builder.AddCell(new PitCell(coordinate.x, coordinate.y));
            builder.AddPlatform(coordinate);
        }

        private void MakeBonus(LevelBuilder builder, IntVector2 coordinate) {
            builder.AddCell(new EmptyCell(coordinate.x, coordinate.y));
            builder.AddBonus(coordinate);
        }

        private void MakePortal(LevelBuilder builder, IntVector2 coordinate) {
            var key = cellDescription["key"];
            builder.AddPortal(coordinate, key);
        }
        
        private void MakeDoor(LevelBuilder builder, IntVector2 coordinate) {
            var key = cellDescription["symbol"];
            builder.AddDoor(coordinate, key);
        }

        private void MakeKey(LevelBuilder builder, IntVector2 coordinate) {
            builder.AddCell(new EmptyCell(coordinate.x, coordinate.y));
            var key = cellDescription["lock"];
            builder.AddKey(coordinate, key);
        }

        private void MakeLaser(LevelBuilder builder, IntVector2 coordinate) {
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

        private void MakeSpear(LevelBuilder builder, IntVector2 coordinate) {
			int onPeriod, offPeriod, currentPeriod;
			bool isOn;
			
			ReadPeriodInfo(out onPeriod, out offPeriod, out currentPeriod, out isOn);

			builder.AddCell(new SpearCell(coordinate.x, coordinate.y, onPeriod, offPeriod, currentPeriod, isOn));
		}

        private void MakeTimeBonus(LevelBuilder builder, IntVector2 coordinate) {
			int levelTickNum = cellDescription["tick"].AsInt;

			builder.AddCell(new EmptyCell(coordinate.x, coordinate.y));
			builder.AddTimeBonus(coordinate, new LevelTick(levelTickNum));
		}

        // TODO rewrite this
		private void ReadPeriodInfo(out int onPeriod, out int offPeriod, out int currentPeriod, out bool isOn) {
            onPeriod = cellDescription["onPeriod"].AsBool ? cellDescription["onPeriod"].AsInt : 1;
			offPeriod = cellDescription["offPeriod"].AsBool ? cellDescription["offPeriod"].AsInt : 1;
			currentPeriod = cellDescription["currentPeriod"].AsBool ? cellDescription["currentPeriod"].AsInt : 0;
			isOn = (cellDescription["currentState"].Value == "on");
		}
        
	}
}
