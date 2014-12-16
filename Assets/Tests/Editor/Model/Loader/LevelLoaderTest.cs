using NUnit.Framework;
using TrappedGame.Model.Cells;
using TrappedGame.Model.LevelLoader.Ascii;

namespace TrappedGame.UnitTests.Model.Loader {
    [TestFixture]
    public class LevelLoaderTest {

        public static readonly string TEST_LEVEL_NAME = "TestLevel";

        [Test]
        public void LoadTest() {
			var levelLoader = new AsciiLevelLoader();
            var level = levelLoader.LoadLevel(TEST_LEVEL_NAME);

            Assert.AreEqual(5, level.SizeX);
            Assert.AreEqual(4, level.SizeY);

            Assert.AreEqual(0, level.StartX);
            Assert.AreEqual(3, level.StartY);
            
            Assert.AreEqual(4, level.FinishX);
            Assert.AreEqual(3, level.FinishY);

            Assert.AreEqual(3, level.Bonuses.Count);

            for (int x = 0; x < 4; x++) {
                Assert.IsInstanceOf<EmptyCell>(level.GetCell(x, 0));
                Assert.IsInstanceOf<EmptyCell>(level.GetCell(x, 3));
            }
            for (int y = 0; y < 3; y++) {
                Assert.IsInstanceOf<EmptyCell>(level.GetCell(0, y));
                Assert.IsInstanceOf<EmptyCell>(level.GetCell(4, y));
            }

            Assert.IsInstanceOf<LaserCell>(level.GetCell(1, 2));
            var laser_1_2 = (LaserCell) level.GetCell(1,2); 
            Assert.AreEqual(true, laser_1_2.Up);
            Assert.AreEqual(true, laser_1_2.Right);
            Assert.AreEqual(true, laser_1_2.Down);
            Assert.AreEqual(true, laser_1_2.Left);

            Assert.IsInstanceOf<SpearCell>(level.GetCell(2, 2));

            Assert.IsInstanceOf<LaserCell>(level.GetCell(1, 1));
            var laser_1_1 = (LaserCell) level.GetCell(1,1); 
            Assert.AreEqual(false, laser_1_1.Up);
            Assert.AreEqual(true, laser_1_1.Right);
            Assert.AreEqual(true, laser_1_1.Down);
            Assert.AreEqual(true, laser_1_1.Left);
            Assert.AreEqual(false, laser_1_1.IsOn);

            Assert.IsInstanceOf<SpearCell>(level.GetCell(2, 1));
            var spear_2_1 = (SpearCell) level.GetCell(2,1); 
            Assert.AreEqual(true, spear_2_1.IsOn);

            Assert.IsInstanceOf<LaserCell>(level.GetCell(3, 1));
            var laser_3_1 = (LaserCell) level.GetCell(3,1); 
            Assert.AreEqual(false, laser_3_1.Up);
            Assert.AreEqual(true, laser_3_1.Right);
            Assert.AreEqual(true, laser_3_1.Down);
            Assert.AreEqual(false, laser_3_1.Left);
            Assert.AreEqual(true, laser_3_1.IsOn);
        }
    }
}