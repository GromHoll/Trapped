using NUnit.Framework;
using TrappedGame.Model.Cells;

namespace TrappedGame.UnitTests.Model.Cells {
    [TestFixture]
    public class WallCellTest {
        [Test]
        public void CreateTest() {
            var wall = new WallCell(1, -1); 
            
            Assert.AreEqual(1, wall.X);
            Assert.AreEqual(-1, wall.Y);
            Assert.AreEqual(CellType.WALL, wall.CellType);
            
            Assert.IsTrue(wall.IsBlocked());
            Assert.IsFalse(wall.IsDeadly());
        }
    }
}

