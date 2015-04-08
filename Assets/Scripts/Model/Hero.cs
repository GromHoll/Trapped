using System;
using System.Collections;
using System.Collections.Generic;
using TrappedGame.Model.Common;
using TrappedGame.Model.Elements;
using TrappedGame.Utils.Observer;

namespace TrappedGame.Model {
    public class Hero : MovableElement {

        public bool IsDead { get; private set; }
        public int DeathCount { get; private set; }

        private IList<Action> deathActions = new List<Action>();

        public Hero(int x, int y) {
            position = new IntVector2(x, y);
        }

        private void NotifyDeathActions() {
            foreach (Action action in deathActions) {
                action.Invoke();
            }
        }

        public void AddDeathAction(Action action) {
            deathActions.Add(action);
        }
        
        public void SetDead(bool isDead) {
            if (IsDead != isDead) {
                IsDead = isDead;
                if (IsDead) {
                    DeathCount++;
                    NotifyDeathActions();
                }
            }
        }

    }
}