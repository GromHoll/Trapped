using NUnit.Framework;
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

            Assert.AreEqual(x, laser.GetX());
            Assert.AreEqual(y, laser.GetY());
            Assert.AreEqual(up, laser.IsUp());
            Assert.AreEqual(right, laser.IsRight());
            Assert.AreEqual(down, laser.IsDown());
            Assert.AreEqual(left, laser.IsLeft());
            Assert.AreEqual(CellType.LASER, laser.GetCellType());
            
            Assert.IsTrue(laser.IsBocked());
            Assert.IsFalse(laser.IsDeadly());
        }

    }
}