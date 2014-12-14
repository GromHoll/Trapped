using TrappedGame.Model.LevelUtils;
using TrappedGame.Model.Cells;
using TrappedGame.Model.Common;
using SimpleJSON;

namespace TrappedGame.Model.LevelLoader.Json {
	public class DefaultCellBuilder {
		public virtual void MakeCell(JSONNode description, LevelBuilder builder, IntVector2 coordinate) {
			string element = description["element"].Value;

			switch(element[0]) {
				case '.' : MakeEmpty(builder, coordinate);     break;
				case '#' : MakeWall(builder, coordinate);      break;
				case 's' : MakeStart(builder, coordinate);     break;
				case 'f' : MakeFinish(builder, coordinate);    break;
				case 'b' : MakeBonus(builder, coordinate);     break;
				case 'S' : MakeSpear(builder, coordinate);     break;
				case 'L' : MakeLaser(builder, coordinate);     break;
				case 'T' : MakeTimeBonus(builder, coordinate); break;
			default : break; //TODO Not support default symbol exception
			}
		}

		protected void MakeEmpty(LevelBuilder builder, IntVector2 coordinate) {
			builder.AddCell(new Cell(coordinate.x, coordinate.y, CellType.EMPTY));
		}

		protected void MakeWall(LevelBuilder builder, IntVector2 coordinate) {
			builder.AddCell(new WallCell(coordinate.x, coordinate.y));
		}

		protected void MakeStart(LevelBuilder builder, IntVector2 coordinate) {
			builder.AddCell(new Cell(coordinate.x, coordinate.y, CellType.EMPTY));
			builder.SetStart(coordinate.x, coordinate.y);
		}

		protected void MakeFinish(LevelBuilder builder, IntVector2 coordinate) {
			builder.AddCell(new Cell(coordinate.x, coordinate.y, CellType.EMPTY));
			builder.SetFinish(coordinate.x, coordinate.y);
		}

		protected void MakeBonus(LevelBuilder builder, IntVector2 coordinate) {
			builder.AddCell(new Cell(coordinate.x, coordinate.y, CellType.EMPTY));
			builder.AddBonus(coordinate);
		}

		protected virtual void MakeLaser(LevelBuilder builder, IntVector2 coordinate) {
			builder.AddCell(new LaserCell(coordinate.x, coordinate.y, true, true, true, true));
		}

		protected virtual void MakeSpear(LevelBuilder builder, IntVector2 coordinate) {
			builder.AddCell(new SpearCell(coordinate.x, coordinate.y));
		}

		protected virtual void MakeTimeBonus(LevelBuilder builder, IntVector2 coordinate) {
			builder.AddCell(new Cell(coordinate.x, coordinate.y, CellType.EMPTY));
			builder.AddTimeBonus(coordinate, LevelTick.FREEZE_LEVEL_TICK);
		}
	}
}