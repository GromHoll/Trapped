using TrappedGame.Model.LevelUtils;
using TrappedGame.Model.Cells;
using TrappedGame.Model.Common;
using SimpleJSON;
using UnityEngine;

namespace TrappedGame.Model.LevelLoader.Json {
	public class DefaultCellBuilder {
		public virtual void MakeCell(JSONNode description, LevelBuilder builder, IntVector2 coordinate) {
			var element = description["element"].Value;

			switch(element[0]) {
                case 's': MakeStart(builder, coordinate);       break;
                case 'f': MakeFinish(builder, coordinate);      break;
				case '#': MakeWall(builder, coordinate);        break;
				case '.': MakeEmpty(builder, coordinate);       break;
				case 'S': MakeSpear(builder, coordinate);       break;
                case 'L': MakeLaser(builder, coordinate);       break;  
                case 'p': MakePit(builder, coordinate);         break;
                case 'b': MakeBonus(builder, coordinate);       break;
                case 'T': MakeTimeBonus(builder, coordinate);   break;
			    default : Debug.Log("Unknown default symbol");  break;
			}
		}

        protected virtual void MakeStart(LevelBuilder builder, IntVector2 coordinate) {
			builder.AddCell(new EmptyCell(coordinate.x, coordinate.y));
			builder.SetStart(coordinate.x, coordinate.y);
		}

        protected virtual void MakeFinish(LevelBuilder builder, IntVector2 coordinate) {
            builder.AddCell(new EmptyCell(coordinate.x, coordinate.y));
			builder.SetFinish(coordinate.x, coordinate.y);
		}

		protected virtual void MakeEmpty(LevelBuilder builder, IntVector2 coordinate) {
            builder.AddCell(new EmptyCell(coordinate.x, coordinate.y));
		}

        protected virtual void MakeWall(LevelBuilder builder, IntVector2 coordinate) {
			builder.AddCell(new WallCell(coordinate.x, coordinate.y));
		}

        protected virtual void MakePit(LevelBuilder builder, IntVector2 coordinate) {
            builder.AddCell(new PitCell(coordinate.x, coordinate.y));
        }

		protected virtual void MakeLaser(LevelBuilder builder, IntVector2 coordinate) {
			builder.AddCell(new LaserCell(coordinate.x, coordinate.y, true, true, true, true));
		}

		protected virtual void MakeSpear(LevelBuilder builder, IntVector2 coordinate) {
			builder.AddCell(new SpearCell(coordinate.x, coordinate.y));
		}

        protected virtual void MakeBonus(LevelBuilder builder, IntVector2 coordinate) {
            builder.AddCell(new EmptyCell(coordinate.x, coordinate.y));
			builder.AddBonus(coordinate);
		}

		protected virtual void MakeTimeBonus(LevelBuilder builder, IntVector2 coordinate) {
            builder.AddCell(new EmptyCell(coordinate.x, coordinate.y));
			builder.AddTimeBonus(coordinate, LevelTick.FREEZE_LEVEL_TICK);
		}
	}
}