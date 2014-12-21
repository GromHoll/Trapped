using System;
using System.Linq;
using TrappedGame.Model.Cells;

namespace TrappedGame.Model {
    public class Game {

        public Level Level { get; private set; }
        public Hero Hero { get; private set; }
        
        public Game(Level level) {
            if (level == null) throw new ArgumentNullException("level");
            Level = level;
            Hero = new Hero(level.StartX, level.StartY);
        }

        public bool IsWin() {
            return !Hero.IsDead && Hero.X == Level.FinishX && Hero.Y == Level.FinishY;
        }

        public int GetScore() {
            var bonuses = Level.Bonuses;
            var path = Hero.Path;
            return path.Links.Count(link => bonuses.Contains(link.From));
        }

        public void MoveHeroUp() {
            MoveHeroTo(Hero.X, Hero.Y + 1);
        }

        public void MoveHeroRight() {
            MoveHeroTo(Hero.X + 1, Hero.Y);
        }

        public void MoveHeroDown() {
            MoveHeroTo(Hero.X, Hero.Y - 1);
        }

        public void MoveHeroLeft() {
            MoveHeroTo(Hero.X - 1, Hero.Y);
        }

        private void MoveHeroTo(int x, int y) {
            if (!HeroOnMap(x, y)) return;
            if (!IsAvailableForMovementCell(x, y)) return;

            if (IsBackTurn(x, y)) {
                MoveBack();
            } else {
                MoveForward(x, y);
            }
            CheckDeadlyCell();
        }

        private void MoveBack() {
            var levelTick = Level.GetLevelTick(Hero.X, Hero.Y);
            levelTick.BackTick(Level);

            var platform = Level.GetPlatform(Hero.X, Hero.Y);
            if (platform != null) {
                platform.MoveBack();
            }
            
            Hero.MoveBack();    
        }

        private void MoveForward(int x, int y) {
            if (Hero.IsDead) return;
            if (HeroWasHere(x, y)) return;

            var levelTick = Level.GetLevelTick(x, y);
            levelTick.NextTick(Level);

            var platform = Level.GetPlatform(Hero.X, Hero.Y);
            if (platform != null) {
                var cell = Level.GetCell(x, y);
                if (cell is PitCell && Level.GetPlatform(x, y) == null) {
                    platform.MoveTo(x, y);
                }
            }

            Hero.MoveTo(x, y);
        }

        private void CheckDeadlyCell() {
            var x = Hero.X;
            var y = Hero.Y;
            var isDanger = Level.IsDeadlyCell(x, y);
            Hero.SetDead(isDanger);
        }

        private bool HeroOnMap(int x, int y) {
            return Level.Contains(x, y); 
        }

        private bool IsAvailableForMovementCell(int x, int y) {
            var cell = Level.GetCell(x, y);
            return !cell.IsBlocked();
        }

        private bool IsBackTurn(int x, int y) {
            var lastTurn = Hero.GetPreviousTurn();
            return lastTurn != null && lastTurn.IsFrom(x, y);
        }

        private bool HeroWasHere(int x, int y) {
            return Hero.WasHere(x, y);
        }
    }
}