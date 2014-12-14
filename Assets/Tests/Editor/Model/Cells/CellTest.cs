using NUnit.Framework;
using TrappedGame.Model.Cells;

namespace TrappedGame.UnitTests.Model.Cells {
    [TestFixture]
    public class CellTest {
        [Test]
        public void CreateTest() {
			var cell = new Cell(5, 6); 

			Assert.AreEqual(5, cell.X);
			Assert.AreEqual(6, cell.Y);
			Assert.AreEqual(CellType.EMPTY, cell.CellType);

			Assert.IsFalse(cell.IsBlocked());
			Assert.IsFalse(cell.IsDeadly());
        }
    }
}