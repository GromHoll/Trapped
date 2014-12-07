using NUnit.Framework;
using TrappedGame.Model.Cells;
using UnityEngine;
using System;

namespace TrappedGame.UnitTests {
    [TestFixture]
    public class LaserCellTest {
        [Test]
        public void CreateTest() {
            TestLaser(0, 0, false, false, false, false);
            TestLaser(0, 0, true, true, true, true);
            TestLaser(0, 0, false, true, false, true);
            TestLaser(0, 0, true, false, true, false);
            TestLaser(-1, 1, true, false, false, true);
            TestLaser(1, -1, false, true, true, false);
        }

        private void TestLaser(int x, int y, bool up, bool right, bool down, bool left) {
            var laser = new LaserCell(x, y, up, right, down, left);

            Assert.AreEqual(x, laser.X);
            Assert.AreEqual(y, laser.Y);
            Assert.AreEqual(up, laser.Up);
            Assert.AreEqual(right, laser.Right);
            Assert.AreEqual(down, laser.Down);
            Assert.AreEqual(left, laser.Left);
            Assert.AreEqual(CellType.LASER, laser.CellType);
            
            Assert.IsTrue(laser.IsBocked());
            Assert.IsFalse(laser.IsDeadly());
        }

    }
}