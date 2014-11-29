using NUnit.Framework;
using System;

namespace TrappedGame.UnitTests {
    [TestFixture()]
    public class IntRectTest {
        [Test()]
        public void ContainsTest() {
            // 3 . . . . . . *
            // 2 . x x x * x .
            // 1 . x * . . x *
            // 0 . x x x x x . 
            //-1 * . . * . . . 
            //  -2-1 0 1 2 3 4
            IntRect rect = new IntRect(-1, 0, 3, 2);

            Assert.IsTrue(rect.Contains(0, 1));
            Assert.IsTrue(rect.Contains(2, 2));
            
            Assert.IsFalse(rect.Contains(-2, -1));
            Assert.IsFalse(rect.Contains(1, -1));
            Assert.IsFalse(rect.Contains(4, 3));
            Assert.IsFalse(rect.Contains(4, 4));
        }
    }
}

