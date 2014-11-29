using System;
using TrappedGame.Model.Cells;
using TrappedGame.Model.Common;
using UnityEngine;
using System.Collections.Generic;


namespace TrappedGame {
    public class Game {

        private Level level;
        private Hero hero;

        private IList<HeroMovementListener> heroMovementListeners = new List<HeroMovementListener>();

        public Game(Level level) {
            if (level == null) throw new ArgumentNullException("level"); 
            this.level = level;
            this.hero = new Hero(level.GetStartX(), level.GetStartY());
        }

        public bool IsWin() {
            return hero.GetX() == level.GetFinishX() && hero.GetY() == level.GetFinishY();
        }

        public int GetScore() {
            int score = 0;
            IList<IntVector2> bonuses = level.GetBonuses();
            Path path = hero.GetPath();
            foreach(Path.PathLink link in path.GetLinks()) {
                if (bonuses.Contains(link.GetFrom())) {
                    score++;
                }
            }
            return score;
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
                LevelTick levelTick = level.GetLevelTick(hero.GetX(), hero.GetY());
                levelTick.BackTick(level);
                hero.MoveBack();
            } else {
                if (hero.IsDead()) return;
                if (HeroWasHere(x, y)) return;                
                LevelTick levelTick = level.GetLevelTick(x, y);
                levelTick.NextTick(level);
                hero.MoveTo(x, y);
            }
            NotifyHeroMovementListener();
            CheckCell();
        }

        private void CheckCell() {
            int x = hero.GetX();
            int y = hero.GetY();
            bool isDanger = level.IsDangerCell(x, y);
            hero.SetDead(isDanger);
        }

        private bool HeroOnMap(int x, int y) {
            return level.Contains(x, y); 
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

        public void AddHeroMovementListener(HeroMovementListener listener) {
            heroMovementListeners.Add(listener);
        }        
        
        public void RemoveHeroMovementListener(HeroMovementListener listener) {
            heroMovementListeners.Remove(listener);
        }        
        
        private void NotifyHeroMovementListener() {
            foreach(HeroMovementListener listener in heroMovementListeners) {
                listener.HeroMoved(hero);
            }
        }
    }
}