using NUnit.Framework;
using TrappedGame.Model.Cells;
using UnityEngine;
using System;

namespace TrappedGame.UnitTests {
    [TestFixture]
    public class SpearCellTest {
        [Test]
        public void SimpleCreateTest() {
            var spear = new SpearCell(2, 3); 
            
            Assert.AreEqual(2, spear.X);
            Assert.AreEqual(3, spear.Y);
            Assert.AreEqual(CellType.SPEAR, spear.CellType);
            
            Assert.IsFalse(spear.IsBlocked());
            Assert.IsFalse(false);
        }

        [Test]
        public void FullCreateTest() {
            SpearTest(2, 3, false);
            SpearTest(2, -1, true);
        }

        private void SpearTest(int x, int y, bool state) {
            var spear = new SpearCell(x, y, 1, 1, 0, state); 
            AssertSpear(spear, x, y, state);
        }

        private void AssertSpear(SpearCell spear, int x, int y, bool state) {            
            Assert.AreEqual(x, spear.X);
            Assert.AreEqual(y, spear.Y);
            AssertSpear(spear, state);
        }

        private void AssertSpear(SpearCell spear, bool state) {
            Assert.AreEqual(CellType.SPEAR, spear.CellType);            
            Assert.AreEqual(state, spear.IsOn);
            Assert.AreEqual(state, spear.IsDeadly());
            Assert.IsFalse(spear.IsBlocked());
        }

        [Test]
        public void NexTickTest() {
            var spear = new SpearCell(0, 0, 1, 2, 1, false); 

            AssertSpear(spear, false);

            spear.NextTick();
            AssertSpear(spear, true);
            
            spear.NextTick();
            AssertSpear(spear, false);
            
            spear.NextTick();
            AssertSpear(spear, false);

            spear.NextTick();
            AssertSpear(spear, true);
        }
        
        [Test]
        public void BackTickTest() {
            var spear = new SpearCell(0, 0, 2, 2, 0, true); 
            
            AssertSpear(spear, true);
            
            spear.BackTick();
            AssertSpear(spear, false);
            
            spear.BackTick();
            AssertSpear(spear, false);
            
            spear.BackTick();
            AssertSpear(spear, true);
        }

    }
}

