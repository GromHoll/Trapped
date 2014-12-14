using NUnit.Framework;
using System;
using TrappedGame.Model;
using TrappedGame.Model.Cells;
using TrappedGame.Model.LevelLoader;
using TrappedGame.Model.LevelLoader.Ascii;

namespace TrappedGame.UnitTests.Model.Loader {
    [TestFixture]
    public class LevelLoaderTest {

        public static readonly string TEST_LEVEL_NAME = "TestLevel";

        [Test]
        public void LoadTest() {
			AsciiLevelLoader levelLoader = new AsciiLevelLoader();
            Level level = levelLoader.LoadLevel(TEST_LEVEL_NAME);

            Assert.AreEqual(5, level.SizeX);
            Assert.AreEqual(4, level.SizeY);

            Assert.AreEqual(1, level.StartX);
            Assert.AreEqual(3, level.StartY);
            
            Assert.AreEqual(4, level.FinishX);
            Assert.AreEqual(3, level.FinishY);

            Assert.AreEqual(3, level.Bonuses.Count);

            for (int x = 0; x < 4; x++) {
                Assert.AreEqual(CellType.EMPTY, level.GetCell(x,0).CellType);
                Assert.AreEqual(CellType.EMPTY, level.GetCell(x,3).CellType);
            }
            for (int y = 0; y < 3; y++) {
                Assert.AreEqual(CellType.EMPTY, level.GetCell(0,y).CellType);
                Assert.AreEqual(CellType.EMPTY, level.GetCell(4,y).CellType);
            }
            
            Assert.AreEqual(CellType.LASER, level.GetCell(1,2).CellType);
            LaserCell laser_1_2 = (LaserCell) level.GetCell(1,2); 
            Assert.AreEqual(true, laser_1_2.Up);
            Assert.AreEqual(true, laser_1_2.Right);
            Assert.AreEqual(true, laser_1_2.Down);
            Assert.AreEqual(true, laser_1_2.Left);

            Assert.AreEqual(CellType.SPEAR, level.GetCell(2,2).CellType);

            Assert.AreEqual(CellType.LASER, level.GetCell(1,1).CellType);
            LaserCell laser_1_1 = (LaserCell) level.GetCell(1,1); 
            Assert.AreEqual(false, laser_1_1.Up);
            Assert.AreEqual(true, laser_1_1.Right);
            Assert.AreEqual(true, laser_1_1.Down);
            Assert.AreEqual(true, laser_1_1.Left);
            Assert.AreEqual(false, laser_1_1.IsOn);
            
            Assert.AreEqual(CellType.SPEAR, level.GetCell(2,1).CellType);
            SpearCell spear_2_1 = (SpearCell) level.GetCell(2,1); 
            Assert.AreEqual(true, spear_2_1.IsOn);

            Assert.AreEqual(CellType.LASER, level.GetCell(3,1).CellType);
            LaserCell laser_3_1 = (LaserCell) level.GetCell(3,1); 
            Assert.AreEqual(false, laser_3_1.Up);
            Assert.AreEqual(true, laser_3_1.Right);
            Assert.AreEqual(true, laser_3_1.Down);
            Assert.AreEqual(false, laser_3_1.Left);
            Assert.AreEqual(true, laser_3_1.IsOn);
        }
    }
}