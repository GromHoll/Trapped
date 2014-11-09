using NUnit.Framework;
using UnityEngine;
using System;

namespace AssemblyCSharpEditor {
    [TestFixture]
    public class CountCellTest {
        [Test]
        public void CreateTest() {
            var cell = new CountCell(new Vector2(1, 2), CellType.UNKNOWN);

            Assert.AreEqual(1, cell.GetX());
            Assert.AreEqual(2, cell.GetY());
            Assert.AreEqual(CellType.UNKNOWN, cell.GetCellType());
            
            Assert.IsFalse(cell.IsOn());   
            Assert.IsFalse(cell.IsBocked());
            Assert.IsFalse(cell.IsDeadly());
        }
        
        [Test]
        public void NextTickTest() {
            var on = 3;
            var off = 2;
            var current = 1;
           
            var cell = new CountCell(Vector2.zero, CellType.UNKNOWN, on, off, current, false);

            Assert.IsFalse(cell.IsOn());

            cell.NextTick();
            Assert.IsTrue(cell.IsOn());
            
            cell.NextTick();
            Assert.IsTrue(cell.IsOn());
            
            cell.NextTick();
            Assert.IsTrue(cell.IsOn());
            
            cell.NextTick();
            Assert.IsFalse(cell.IsOn());
            
            cell.NextTick();
            Assert.IsFalse(cell.IsOn());
            
            cell.NextTick();
            Assert.IsTrue(cell.IsOn());
        }

        [Test]
        public void BackTickTest() {
            var on = 1;
            var off = 2;
            var current = 0;
            
            var cell = new CountCell(Vector2.zero, CellType.UNKNOWN, on, off, current, true);
            
            Assert.IsTrue(cell.IsOn());
            
            cell.BackTick();
            Assert.IsFalse(cell.IsOn());
            
            cell.BackTick();
            Assert.IsFalse(cell.IsOn());
            
            cell.BackTick();
            Assert.IsTrue(cell.IsOn());
            
            cell.BackTick();
            Assert.IsFalse(cell.IsOn());
            
            cell.BackTick();
            Assert.IsFalse(cell.IsOn());
        }
    }
}

