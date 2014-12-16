using NUnit.Framework;
using TrappedGame.Model.Cells;

namespace TrappedGame.UnitTests.Model.Cells {
    [TestFixture]
    public class CellTest {
        [Test]
        public void CreateTest() {
			var cell = new EmptyCell(5, 6); 

			Assert.AreEqual(5, cell.X);
			Assert.AreEqual(6, cell.Y);

			Assert.IsFalse(cell.IsBlocked());
			Assert.IsFalse(cell.IsDeadly());
        }
    }
}