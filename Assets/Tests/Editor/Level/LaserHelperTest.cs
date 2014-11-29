using NUnit.Framework;
using System;

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

            LaserCell.Laser infinityUp = helper.CreateUpLaser(infinityLaser, level);
            Assert.AreEqual(2, infinityUp.GetCover().GetMinX());
            Assert.AreEqual(4, infinityUp.GetCover().GetMinY());
            Assert.AreEqual(2, infinityUp.GetCover().GetMaxX());
            Assert.AreEqual(5, infinityUp.GetCover().GetMaxY());
                        
            LaserCell.Laser infinityRight = helper.CreateRightLaser(infinityLaser, level);
            Assert.AreEqual(3, infinityRight.GetCover().GetMinX());
            Assert.AreEqual(3, infinityRight.GetCover().GetMinY());
            Assert.AreEqual(5, infinityRight.GetCover().GetMaxX());
            Assert.AreEqual(3, infinityRight.GetCover().GetMaxY());            

            LaserCell.Laser infinityDown = helper.CreateDownLaser(infinityLaser, level);
            Assert.AreEqual(2, infinityDown.GetCover().GetMinX());
            Assert.AreEqual(0, infinityDown.GetCover().GetMinY());
            Assert.AreEqual(2, infinityDown.GetCover().GetMaxX());
            Assert.AreEqual(2, infinityDown.GetCover().GetMaxY());            

            LaserCell.Laser infinityLeft = helper.CreateLeftLaser(infinityLaser, level);
            Assert.AreEqual(0, infinityLeft.GetCover().GetMinX());
            Assert.AreEqual(3, infinityLeft.GetCover().GetMinY());
            Assert.AreEqual(1, infinityLeft.GetCover().GetMaxX());
            Assert.AreEqual(3, infinityLeft.GetCover().GetMaxY());
            
            LaserCell blockedLaser = (LaserCell) level.GetCell(3, 2);

            LaserCell.Laser blockedUp = helper.CreateUpLaser(blockedLaser, level);
            Assert.AreEqual(3, blockedUp.GetCover().GetMinX());
            Assert.AreEqual(3, blockedUp.GetCover().GetMinY());
            Assert.AreEqual(3, blockedUp.GetCover().GetMaxX());
            Assert.AreEqual(4, blockedUp.GetCover().GetMaxY());

            LaserCell.Laser blockedRight = helper.CreateRightLaser(blockedLaser, level);
            Assert.AreEqual(4, blockedRight.GetCover().GetMinX());
            Assert.AreEqual(2, blockedRight.GetCover().GetMinY());
            Assert.AreEqual(4, blockedRight.GetCover().GetMaxX());
            Assert.AreEqual(2, blockedRight.GetCover().GetMaxY());

            LaserCell.Laser blockedDown = helper.CreateDownLaser(blockedLaser, level);
            Assert.AreEqual(3, blockedDown.GetCover().GetMinX());
            Assert.AreEqual(1, blockedDown.GetCover().GetMinY());
            Assert.AreEqual(3, blockedDown.GetCover().GetMaxX());
            Assert.AreEqual(1, blockedDown.GetCover().GetMaxY());

            LaserCell.Laser blockedLeft = helper.CreateLeftLaser(blockedLaser, level);
            Assert.AreEqual(1, blockedLeft.GetCover().GetMinX());
            Assert.AreEqual(2, blockedLeft.GetCover().GetMinY());
            Assert.AreEqual(2, blockedLeft.GetCover().GetMaxX());
            Assert.AreEqual(2, blockedLeft.GetCover().GetMaxY());
        }
    }
}

