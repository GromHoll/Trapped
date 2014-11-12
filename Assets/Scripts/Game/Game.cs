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
            Debug.Log("Move hero to = (" + x + ", " + y + ")");
            if (!HeroOnMap(x, y)) return;
            Debug.Log("Hero on map");
            if (!IsAvailableForMovementCell(x, y)) return;
            Debug.Log("Cell not blocked");
            if (IsBackTurn(x, y)) {
                Debug.Log("Is back turn");
                hero.MoveBack();
                level.BackTick();
            } else {
                Debug.Log("Is next turn");
                if (hero.IsDead()) return;
                Debug.Log("Hero not dead");
                if (HeroWasHere(x, y)) return;
                Debug.Log("Hero not was here");
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