using NUnit.Framework;
using TrappedGame.Model.Cells;
using System;

namespace TrappedGame.UnitTests {
    [TestFixture]
    public class CountCellTest {
        [Test]
        public void CreateTest() {
            var cell = new CountCell(1, 2, CellType.UNKNOWN);

            Assert.AreEqual(1, cell.X);
            Assert.AreEqual(2, cell.Y);
            Assert.AreEqual(CellType.UNKNOWN, cell.CellType);
            
            Assert.IsFalse(cell.IsOn);   
            Assert.IsFalse(cell.IsBocked());
            Assert.IsFalse(cell.IsDeadly());
        }
        
        [Test]
        public void NextTickTest() {
            var on = 3;
            var off = 2;
            var current = 1;
           
            var cell = new CountCell(0, 0, CellType.UNKNOWN, on, off, current, false);

            Assert.IsFalse(cell.IsOn);
            Assert.IsTrue(cell.IsOnOnNextTick());

            cell.NextTick();
            Assert.IsTrue(cell.IsOn);
            Assert.IsTrue(cell.IsOnOnNextTick());
            
            cell.NextTick();
            Assert.IsTrue(cell.IsOn);
            Assert.IsTrue(cell.IsOnOnNextTick());
            
            cell.NextTick();
            Assert.IsTrue(cell.IsOn);
            Assert.IsFalse(cell.IsOnOnNextTick());
            
            cell.NextTick();
            Assert.IsFalse(cell.IsOn);
            Assert.IsFalse(cell.IsOnOnNextTick());
            
            cell.NextTick();
            Assert.IsFalse(cell.IsOn);
            Assert.IsTrue(cell.IsOnOnNextTick());
            
            cell.NextTick();
            Assert.IsTrue(cell.IsOn);
        }

        [Test]
        public void BackTickTest() {
            var on = 1;
            var off = 2;
            var current = 0;
            
            var cell = new CountCell(0, 0, CellType.UNKNOWN, on, off, current, true);
            
            Assert.IsTrue(cell.IsOn);
            
            cell.BackTick();
            Assert.IsFalse(cell.IsOn);
            
            cell.BackTick();
            Assert.IsFalse(cell.IsOn);
            
            cell.BackTick();
            Assert.IsTrue(cell.IsOn);
            
            cell.BackTick();
            Assert.IsFalse(cell.IsOn);
            
            cell.BackTick();
            Assert.IsFalse(cell.IsOn);
        }
		
		[Test]
        public void IllegaCurrentPeriodTest() {
            Assert.Throws(typeof(ArgumentException),
                          new TestDelegate(IllegaCurrentPeriod));
        }

        private void IllegaCurrentPeriod() {
            new CountCell(1, 1, CellType.UNKNOWN, 1, 1, 2, true);
        }
        
        [Test]
        public void IllegaOnPeriodTest() {
            Assert.Throws(typeof(ArgumentException),
                          new TestDelegate(IllegaOnPeriod));
        }
        
        private void IllegaOnPeriod() {
            new CountCell(0, 1, CellType.UNKNOWN, 0, 1, 0, true);
        }
        
        [Test]
        public void IllegaOffPeriodTest() {
            Assert.Throws(typeof(ArgumentException),
                          new TestDelegate(IllegaOffPeriod));
        }
        
        private void IllegaOffPeriod() {
            new CountCell(1, -1, CellType.UNKNOWN, 1, -1, 0, true);
        }


    }
}
