using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TrappedGame.Model.Cells;
using UnityEngine;

namespace TrappedGame.Model {
    public class Game {

        public Level Level { get; private set; }
        public Hero Hero { get; private set; }

        private IList<Action> wrongTurnActions = new List<Action>();
        
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

        public void AddWrongTurnAAction(Action action) {
            wrongTurnActions.Add(action);
        }

        public void NotifyAboutWrongTurn() {
            foreach (Action action in wrongTurnActions) {
                action.Invoke();
            }
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
            if (!HeroOnMap(x, y) || !IsAvailableForMovementCell(x, y)) {
                NotifyAboutWrongTurn();
                return;
            }

            if (IsBackTurn(x, y)) {
                MoveBack();
            } else {
                MoveForward(x, y);
            }
            CheckDeadlyCell();
        }

        // TODO Refactor this (too hard for undestanding)
        private void MoveBack() {
            var levelTick = Level.GetLevelTick(Hero.X, Hero.Y);
            levelTick.BackTick(Level);

            var platform = Level.GetPlatform(Hero.X, Hero.Y);
            if (platform != null) {
                platform.MoveBack();
            }

            foreach (var key in Level.Keys) {
                if (key.Coordinate == Hero.Coordinate) {
                    key.Drop();
                }    
            }
            
            Hero.MoveBack();

            if (Level.GetCell(Hero.X, Hero.Y) is PortalCell) {
                Hero.MoveBack();
                Hero.MoveBack();
            }
        }

        // TODO Refactor this (too hard for undestanding)
        private void MoveForward(int x, int y) {
            if (Hero.IsDead || HeroWasHere(x, y)) {
                NotifyAboutWrongTurn();
                return;
            }

            var toCell = Level.GetCell(x, y);
            if (toCell is PortalCell) {
                var endPoint = (toCell as PortalCell).EndPoint(Hero.Coordinate);
                if (!HeroOnMap(endPoint.x, endPoint.y) || !IsAvailableForMovementCell(endPoint.x, endPoint.y) || HeroWasHere(endPoint.x, endPoint.y)) {
                    NotifyAboutWrongTurn();
                    return;
                }
            }

            var platform = Level.GetPlatform(Hero.X, Hero.Y);
            if (platform != null) {
                if (toCell is PitCell && Level.GetPlatform(x, y) == null) {
                    platform.MoveTo(x, y);
                }
            }
            
            var levelTick = Level.GetLevelTick(x, y);
            levelTick.NextTick(Level);

            var fromCoordinate = Hero.Coordinate.Clone();
            Hero.MoveTo(x, y);

            if (toCell is PortalCell) {
                var portalCell = toCell as PortalCell;
                Hero.MoveTo(portalCell.Pair.X, portalCell.Pair.Y);
                var endPoint = portalCell.EndPoint(fromCoordinate);
                Hero.MoveTo(endPoint.x, endPoint.y);
            }

            foreach (var key in Level.Keys) {
                if (key.Coordinate == Hero.Coordinate) {
                    key.PickUp();
                }
            }
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