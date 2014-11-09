using NUnit.Framework;
using UnityEngine;
using System;

namespace TrappedUnitTests {
    [TestFixture]
    public class CellTest {
        [Test]
        public void CreateTest() {
			var cell = new Cell(new Vector2(5, 6)); 

			Assert.AreEqual(5, cell.GetX());
			Assert.AreEqual(6, cell.GetY());
			Assert.AreEqual(CellType.EMPTY, cell.GetCellType());

			Assert.IsFalse(cell.IsBocked());
			Assert.IsFalse(cell.IsDeadly());
        }
    }
}