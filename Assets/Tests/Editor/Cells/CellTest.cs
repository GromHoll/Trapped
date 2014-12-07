using NUnit.Framework;
using TrappedGame.Model.Cells;
using UnityEngine;
using System;

namespace TrappedGame.UnitTests {
    [TestFixture]
    public class CellTest {
        [Test]
        public void CreateTest() {
			var cell = new Cell(5, 6); 

			Assert.AreEqual(5, cell.X);
			Assert.AreEqual(6, cell.Y);
			Assert.AreEqual(CellType.EMPTY, cell.CellType);

			Assert.IsFalse(cell.IsBocked());
			Assert.IsFalse(cell.IsDeadly());
        }
    }
}