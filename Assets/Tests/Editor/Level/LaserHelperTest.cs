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
            Assert.AreEqual(2, infinityUp.Cover.GetMinX());
            Assert.AreEqual(4, infinityUp.Cover.GetMinY());
            Assert.AreEqual(2, infinityUp.Cover.GetMaxX());
            Assert.AreEqual(5, infinityUp.Cover.GetMaxY());
                        
            LaserCell.Line infinityRight = helper.CreateRightLaser(infinityLaser, level);
            Assert.AreEqual(3, infinityRight.Cover.GetMinX());
            Assert.AreEqual(3, infinityRight.Cover.GetMinY());
            Assert.AreEqual(5, infinityRight.Cover.GetMaxX());
            Assert.AreEqual(3, infinityRight.Cover.GetMaxY());            

            LaserCell.Line infinityDown = helper.CreateDownLaser(infinityLaser, level);
            Assert.AreEqual(2, infinityDown.Cover.GetMinX());
            Assert.AreEqual(0, infinityDown.Cover.GetMinY());
            Assert.AreEqual(2, infinityDown.Cover.GetMaxX());
            Assert.AreEqual(2, infinityDown.Cover.GetMaxY());            

            LaserCell.Line infinityLeft = helper.CreateLeftLaser(infinityLaser, level);
            Assert.AreEqual(0, infinityLeft.Cover.GetMinX());
            Assert.AreEqual(3, infinityLeft.Cover.GetMinY());
            Assert.AreEqual(1, infinityLeft.Cover.GetMaxX());
            Assert.AreEqual(3, infinityLeft.Cover.GetMaxY());
            
            LaserCell blockedLaser = (LaserCell) level.GetCell(3, 2);

            LaserCell.Line blockedUp = helper.CreateUpLaser(blockedLaser, level);
            Assert.AreEqual(3, blockedUp.Cover.GetMinX());
            Assert.AreEqual(3, blockedUp.Cover.GetMinY());
            Assert.AreEqual(3, blockedUp.Cover.GetMaxX());
            Assert.AreEqual(4, blockedUp.Cover.GetMaxY());

            LaserCell.Line blockedRight = helper.CreateRightLaser(blockedLaser, level);
            Assert.AreEqual(4, blockedRight.Cover.GetMinX());
            Assert.AreEqual(2, blockedRight.Cover.GetMinY());
            Assert.AreEqual(4, blockedRight.Cover.GetMaxX());
            Assert.AreEqual(2, blockedRight.Cover.GetMaxY());

            LaserCell.Line blockedDown = helper.CreateDownLaser(blockedLaser, level);
            Assert.AreEqual(3, blockedDown.Cover.GetMinX());
            Assert.AreEqual(1, blockedDown.Cover.GetMinY());
            Assert.AreEqual(3, blockedDown.Cover.GetMaxX());
            Assert.AreEqual(1, blockedDown.Cover.GetMaxY());

            LaserCell.Line blockedLeft = helper.CreateLeftLaser(blockedLaser, level);
            Assert.AreEqual(1, blockedLeft.Cover.GetMinX());
            Assert.AreEqual(2, blockedLeft.Cover.GetMinY());
            Assert.AreEqual(2, blockedLeft.Cover.GetMaxX());
            Assert.AreEqual(2, blockedLeft.Cover.GetMaxY());
        }
    }
}

