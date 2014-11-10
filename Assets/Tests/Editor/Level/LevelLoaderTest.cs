using NUnit.Framework;
using System;

namespace TrappedUnitTests {
    [TestFixture]
    public class LevelLoaderTest {

        public static readonly string TEST_LEVEL_NAME = "./Assets/Tests/Editor/Level/TestLevel.txt";

        [Test]
        public void LoadTest() {
            LevelLoader levelLoader = new LevelLoader();
            Level level = levelLoader.LoadLevel(TEST_LEVEL_NAME);

            Assert.AreEqual(5, level.xSize);
            Assert.AreEqual(4, level.ySize);

            Assert.AreEqual(0, level.start.x);
            Assert.AreEqual(3, level.start.y);
            
            Assert.AreEqual(4, level.finish.x);
            Assert.AreEqual(3, level.start.y);

            Assert.AreEqual(3, level.bonuses.Count);

            for (int x = 0; x < 4; x++) {
                Assert.AreEqual(CellType.EMPTY, level.cells[x,0].GetCellType());
                Assert.AreEqual(CellType.EMPTY, level.cells[x,3].GetCellType());
            }
            for (int y = 0; y < 3; y++) {
                Assert.AreEqual(CellType.EMPTY, level.cells[0,y].GetCellType());
                Assert.AreEqual(CellType.EMPTY, level.cells[4,y].GetCellType());
            }
            
            Assert.AreEqual(CellType.LASER, level.cells[1,2].GetCellType());
            LaserCell laser_1_2 = (LaserCell) level.cells[1,2]; 
            Assert.AreEqual(true, laser_1_2.IsUp());
            Assert.AreEqual(true, laser_1_2.IsRight());
            Assert.AreEqual(true, laser_1_2.IsDown());
            Assert.AreEqual(true, laser_1_2.IsLeft());

            Assert.AreEqual(CellType.SPEAR, level.cells[2,2].GetCellType());

            Assert.AreEqual(CellType.LASER, level.cells[1,1].GetCellType());
            LaserCell laser_1_1 = (LaserCell) level.cells[1,1]; 
            Assert.AreEqual(false, laser_1_1.IsUp());
            Assert.AreEqual(true, laser_1_1.IsRight());
            Assert.AreEqual(true, laser_1_1.IsDown());
            Assert.AreEqual(true, laser_1_1.IsLeft());
            Assert.AreEqual(false, laser_1_1.IsOn());
            
            Assert.AreEqual(CellType.SPEAR, level.cells[2,1].GetCellType());
            SpearCell spear_2_1 = (SpearCell) level.cells[2,1]; 
            Assert.AreEqual(true, spear_2_1.IsOn());

            Assert.AreEqual(CellType.LASER, level.cells[3,1].GetCellType());
            LaserCell laser_3_1 = (LaserCell) level.cells[3,1]; 
            Assert.AreEqual(false, laser_3_1.IsUp());
            Assert.AreEqual(true, laser_3_1.IsRight());
            Assert.AreEqual(true, laser_3_1.IsDown());
            Assert.AreEqual(false, laser_3_1.IsLeft());
            Assert.AreEqual(true, laser_3_1.IsOn());
        }
    }
}