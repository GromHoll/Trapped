using NUnit.Framework;
using System;
using TrappedGame.Model;
using TrappedGame.Model.Cells;
using TrappedGame.Model.LevelUtils;

namespace TrappedGame.UnitTests {
    [TestFixture]
    public class LaserHelperTest {
        
        private Level CreateLevel() {
            LevelBuilder builder = new LevelBuilder("LaserHelperTest", 6, 6);
            builder.AddCell(new LaserCell(2, 3, true, true, true, true));
            builder.AddCell(new LaserCell(3, 2, true, true, true, true));
            builder.AddCell(new WallCell(3, 0));
            builder.AddCell(new WallCell(0, 2));
            builder.AddCell(new WallCell(5, 2));
            builder.AddCell(new WallCell(3, 5));
            return builder.Build();
        }

        [Test]
        public void CreateLinesTest() {

            Level level = CreateLevel();
            LaserHelper helper = new LaserHelper();

            LaserCell infinityLaser = (LaserCell) level.GetCell(2, 3);

            LaserCell.Line infinityUp = helper.CreateUpLaser(infinityLaser, level);
            Assert.AreEqual(2, infinityUp.Cover.MinX);
            Assert.AreEqual(4, infinityUp.Cover.MinY);
            Assert.AreEqual(2, infinityUp.Cover.MaxX);
            Assert.AreEqual(5, infinityUp.Cover.MaxY);
                        
            LaserCell.Line infinityRight = helper.CreateRightLaser(infinityLaser, level);
            Assert.AreEqual(3, infinityRight.Cover.MinX);
            Assert.AreEqual(3, infinityRight.Cover.MinY);
            Assert.AreEqual(5, infinityRight.Cover.MaxX);
            Assert.AreEqual(3, infinityRight.Cover.MaxY);            

            LaserCell.Line infinityDown = helper.CreateDownLaser(infinityLaser, level);
            Assert.AreEqual(2, infinityDown.Cover.MinX);
            Assert.AreEqual(0, infinityDown.Cover.MinY);
            Assert.AreEqual(2, infinityDown.Cover.MaxX);
            Assert.AreEqual(2, infinityDown.Cover.MaxY);            

            LaserCell.Line infinityLeft = helper.CreateLeftLaser(infinityLaser, level);
            Assert.AreEqual(0, infinityLeft.Cover.MinX);
            Assert.AreEqual(3, infinityLeft.Cover.MinY);
            Assert.AreEqual(1, infinityLeft.Cover.MaxX);
            Assert.AreEqual(3, infinityLeft.Cover.MaxY);
            
            LaserCell blockedLaser = (LaserCell) level.GetCell(3, 2);

            LaserCell.Line blockedUp = helper.CreateUpLaser(blockedLaser, level);
            Assert.AreEqual(3, blockedUp.Cover.MinX);
            Assert.AreEqual(3, blockedUp.Cover.MinY);
            Assert.AreEqual(3, blockedUp.Cover.MaxX);
            Assert.AreEqual(4, blockedUp.Cover.MaxY);

            LaserCell.Line blockedRight = helper.CreateRightLaser(blockedLaser, level);
            Assert.AreEqual(4, blockedRight.Cover.MinX);
            Assert.AreEqual(2, blockedRight.Cover.MinY);
            Assert.AreEqual(4, blockedRight.Cover.MaxX);
            Assert.AreEqual(2, blockedRight.Cover.MaxY);

            LaserCell.Line blockedDown = helper.CreateDownLaser(blockedLaser, level);
            Assert.AreEqual(3, blockedDown.Cover.MinX);
            Assert.AreEqual(1, blockedDown.Cover.MinY);
            Assert.AreEqual(3, blockedDown.Cover.MaxX);
            Assert.AreEqual(1, blockedDown.Cover.MaxY);

            LaserCell.Line blockedLeft = helper.CreateLeftLaser(blockedLaser, level);
            Assert.AreEqual(1, blockedLeft.Cover.MinX);
            Assert.AreEqual(2, blockedLeft.Cover.MinY);
            Assert.AreEqual(2, blockedLeft.Cover.MaxX);
            Assert.AreEqual(2, blockedLeft.Cover.MaxY);
        }
    }
}

