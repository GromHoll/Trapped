using UnityEngine;
using NUnit.Framework;
using System;

namespace TrappedUnitTests {
    [TestFixture]
    public class CellsTest {
        [Test]
        public void CreateTest() {
			Cell cell = new Cell(new Vector2(5, 6)); 

			Assert.AreEqual(5, cell.GetX());
			Assert.AreEqual(6, cell.GetY());
			Assert.AreEqual(CellType.EMPTY, cell.GetCellType());
        }
    }
}