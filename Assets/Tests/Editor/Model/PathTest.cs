using System.Linq;
using NUnit.Framework;
using TrappedGame.Model;

namespace TrappedGame.UnitTests.Model {

    [TestFixture]
    class PathTest {

        [Test]
        public void CreateTest() {
            var path = new Path();
            Assert.IsTrue(path.IsEmpty());
            Assert.IsEmpty(path.Links);

            path.AddLink(0, 0, 1, 0);
            path.AddLink(1, 0, 1, 1);

            Assert.IsFalse(path.IsEmpty());
            Assert.IsNotEmpty(path.Links);
            Assert.AreEqual(2, path.Links.Count());

            var previousTurn = path.GetPreviousTurn();
            Assert.AreEqual(1, previousTurn.FromX);
            Assert.AreEqual(0, previousTurn.FromY);
            Assert.AreEqual(1, previousTurn.ToX);
            Assert.AreEqual(1, previousTurn.ToY);

            path.RemoveLink();
            Assert.IsFalse(path.IsEmpty());
            Assert.IsNotEmpty(path.Links);
            Assert.AreEqual(1, path.Links.Count());

            previousTurn = path.GetPreviousTurn();
            Assert.AreEqual(0, previousTurn.FromX);
            Assert.AreEqual(0, previousTurn.FromY);
            Assert.AreEqual(1, previousTurn.ToX);
            Assert.AreEqual(0, previousTurn.ToY);

            path.RemoveLink();
            Assert.IsTrue(path.IsEmpty());
            Assert.IsEmpty(path.Links);
        }
    }
}
