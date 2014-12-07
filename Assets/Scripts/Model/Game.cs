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
            return Hero.GetX() == Level.FinishX && Hero.GetY() == Level.FinishY;
        }

        public int GetScore() {
            var bonuses = Level.Bonuses;
            var path = Hero.GetPath();
            return path.GetLinks().Count(link => bonuses.Contains(link.GetFrom()));
        }

        public void MoveHeroUp() {
            MoveHeroTo(Hero.GetX(), Hero.GetY() + 1);
        }

        public void MoveHeroRight() {
            MoveHeroTo(Hero.GetX() + 1, Hero.GetY());
        }

        public void MoveHeroDown() {
            MoveHeroTo(Hero.GetX(), Hero.GetY() - 1);
        }

        public void MoveHeroLeft() {
            MoveHeroTo(Hero.GetX() - 1, Hero.GetY());
        }

        private void MoveHeroTo(int x, int y) {
            if (!HeroOnMap(x, y)) return;
            if (!IsAvailableForMovementCell(x, y)) return;

            if (IsBackTurn(x, y)) {
                var levelTick = Level.GetLevelTick(Hero.GetX(), Hero.GetY());
                levelTick.BackTick(Level);
                Hero.MoveBack();
            } else {
                if (Hero.IsDead()) return;
                if (HeroWasHere(x, y)) return;
                var levelTick = Level.GetLevelTick(x, y);
                levelTick.NextTick(Level);
                Hero.MoveTo(x, y);
            }
            NotifyHeroMovementListener();
            CheckCell();
        }

        private void CheckCell() {
            var x = Hero.GetX();
            var y = Hero.GetY();
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