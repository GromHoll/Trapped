using NUnit.Framework;
using UnityEngine;
using System;

namespace TrappedUnitTests {
    [TestFixture]
    public class WallCellTest {
        [Test]
        public void CreateTest() {
            var wall = new WallCell(new Vector2(1, -1)); 
            
            Assert.AreEqual(1, wall.GetX());
            Assert.AreEqual(-1, wall.GetY());
            Assert.AreEqual(CellType.WALL, wall.GetCellType());
            
            Assert.IsTrue(wall.IsBocked());
            Assert.IsFalse(wall.IsDeadly());
        }
    }
}

