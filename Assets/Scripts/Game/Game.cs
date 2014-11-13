using System;
using UnityEngine;


namespace TrappedGame {
    public class Game {

        private Level level;
        private Hero hero;

        public Game(Level level) {
            if (level == null) throw new ArgumentNullException("level"); 
            this.level = level;
            this.hero = new Hero(level.GetStartX(), level.GetStartY());
        }

        public bool IsWin() {
            return level.GetStartX() == level.GetFinishX() && level.GetStartY() == level.GetFinishY();
        }

        public Hero GetHero() {
            return hero;
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
                hero.MoveBack();
                level.BackTick();
            } else {
                if (hero.IsDead()) return;
                if (HeroWasHere(x, y)) return;
                hero.MoveTo(x, y);
                level.NextTick();
            }
        }

        private bool HeroOnMap(int x, int y) {
            return level.contains(x, y); 
        }

        private bool IsAvailableForMovementCell(int x, int y) {
            Cell cell = level.GetCell(x, y);
            return !cell.IsBocked();
        }

        private bool IsBackTurn(int x, int y) {
            var lastTurn = hero.GetPreviousTurn();
            return lastTurn != null ? lastTurn.IsFrom(x, y) : false;
        }

        private bool HeroWasHere(int x, int y) {
            return hero.WasHere(x, y);
        }
    }
}