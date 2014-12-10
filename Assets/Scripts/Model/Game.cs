using System;
using System.Collections.Generic;
using System.Linq;
using TrappedGame.Model.Listeners;

namespace TrappedGame.Model {
    public class Game {

        public Level Level { get; private set; }
        public Hero Hero { get; private set; }

        private readonly IList<IHeroMovementListener> heroMovementListeners = new List<IHeroMovementListener>();

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
                var levelTick = Level.GetLevelTick(Hero.X, Hero.Y);
                levelTick.BackTick(Level);
                Hero.MoveBack();
            } else {
                if (Hero.IsDead) return;
                if (HeroWasHere(x, y)) return;
                var levelTick = Level.GetLevelTick(x, y);
                levelTick.NextTick(Level);
                Hero.MoveTo(x, y);
            }
            NotifyHeroMovementListener();
            CheckCell();
        }

        private void CheckCell() {
            var x = Hero.X;
            var y = Hero.Y;
            var isDanger = Level.IsDangerCell(x, y);
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

        public void AddHeroMovementListener(IHeroMovementListener listener) {
            heroMovementListeners.Add(listener);
        }        
        
        public void RemoveHeroMovementListener(IHeroMovementListener listener) {
            heroMovementListeners.Remove(listener);
        }        
        
        private void NotifyHeroMovementListener() {
            foreach(var listener in heroMovementListeners) {
                listener.HeroMoved(Hero);
            }
        }
    }
}