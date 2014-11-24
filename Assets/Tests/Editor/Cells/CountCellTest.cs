using NUnit.Framework;
using UnityEngine;
using System;


namespace TrappedUnitTests {
    [TestFixture]
    public class CountCellTest {
        [Test]
        public void CreateTest() {
            var cell = new CountCell(1, 2, CellType.UNKNOWN);

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
           
            var cell = new CountCell(0, 0, CellType.UNKNOWN, on, off, current, false);

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
            
            var cell = new CountCell(0, 0, CellType.UNKNOWN, on, off, current, true);
            
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
		
		[Test]
        public void IllegaCurrentPeriodTest() {
            Assert.Throws(typeof(ArgumentException),
                          new TestDelegate(IllegaCurrentPeriod));
        }

        private void IllegaCurrentPeriod() {
            var cell = new CountCell(1, 1, CellType.UNKNOWN, 1, 1, 2, true);
        }
        
        [Test]
        public void IllegaOnPeriodTest() {
            Assert.Throws(typeof(ArgumentException),
                          new TestDelegate(IllegaOnPeriod));
        }
        
        private void IllegaOnPeriod() {
            var cell = new CountCell(0, 1, CellType.UNKNOWN, 0, 1, 0, true);
        }
        
        [Test]
        public void IllegaOffPeriodTest() {
            Assert.Throws(typeof(ArgumentException),
                          new TestDelegate(IllegaOffPeriod));
        }
        
        private void IllegaOffPeriod() {
            var cell = new CountCell(1, -1, CellType.UNKNOWN, 1, -1, 0, true);
        }


    }
}
