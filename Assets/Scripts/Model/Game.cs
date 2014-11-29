using System;
using System.Collections.Generic;
using System.Linq;

namespace TrappedGame.Model {
    public class Game {

        private readonly Level level;
        private readonly Hero hero;

        private readonly IList<HeroMovementListener> heroMovementListeners = new List<HeroMovementListener>();

        public Game(Level level) {
            if (level == null) throw new ArgumentNullException("level"); 
            this.level = level;
            this.hero = new Hero(level.GetStartX(), level.GetStartY());
        }

        public bool IsWin() {
            return hero.GetX() == level.GetFinishX() && hero.GetY() == level.GetFinishY();
        }

        public int GetScore() {
            var bonuses = level.GetBonuses();
            var path = hero.GetPath();
            return path.GetLinks().Count(link => bonuses.Contains(link.GetFrom()));
        }

        public Hero GetHero() {
            return hero;
        }
                
        public Level GetLevel() {
            return level;
        }

        public void MoveHeroUp() {
            MoveHeroTo(hero.GetX(), hero.GetY() + 1);
        }

        public void MoveHeroRight() {
            MoveHeroTo(hero.GetX() + 1, hero.GetY());
        }

        public void MoveHeroDown() {
            MoveHeroTo(hero.GetX(), hero.GetY() - 1);
        }

        public void MoveHeroLeft() {
            MoveHeroTo(hero.GetX() - 1, hero.GetY());
        }

        private void MoveHeroTo(int x, int y) {
            if (!HeroOnMap(x, y)) return;
            if (!IsAvailableForMovementCell(x, y)) return;

            if (IsBackTurn(x, y)) {
                var levelTick = level.GetLevelTick(hero.GetX(), hero.GetY());
                levelTick.BackTick(level);
                hero.MoveBack();
            } else {
                if (hero.IsDead()) return;
                if (HeroWasHere(x, y)) return;                
                var levelTick = level.GetLevelTick(x, y);
                levelTick.NextTick(level);
                hero.MoveTo(x, y);
            }
            NotifyHeroMovementListener();
            CheckCell();
        }

        private void CheckCell() {
            var x = hero.GetX();
            var y = hero.GetY();
            var isDanger = level.IsDangerCell(x, y);
            hero.SetDead(isDanger);
        }

        private bool HeroOnMap(int x, int y) {
            return level.Contains(x, y); 
        }

        private bool IsAvailableForMovementCell(int x, int y) {
            var cell = level.GetCell(x, y);
            return !cell.IsBocked();
        }

        private bool IsBackTurn(int x, int y) {
            var lastTurn = hero.GetPreviousTurn();
            return lastTurn != null && lastTurn.IsFrom(x, y);
        }

        private bool HeroWasHere(int x, int y) {
            return hero.WasHere(x, y);
        }

        public void AddHeroMovementListener(HeroMovementListener listener) {
            heroMovementListeners.Add(listener);
        }        
        
        public void RemoveHeroMovementListener(HeroMovementListener listener) {
            heroMovementListeners.Remove(listener);
        }        
        
        private void NotifyHeroMovementListener() {
            foreach(var listener in heroMovementListeners) {
                listener.HeroMoved(hero);
            }
        }
    }
}