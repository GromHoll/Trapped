using TrappedGame.Model;
using TrappedGame.Utils;
using TrappedGame.View.Controllers.Common;
using TrappedGame.View.Sync;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class HeroController : MovableObjectController, ISyncGameObject {
        
        public static readonly string IS_DEAD_KEY = "IsDead";
        
        private Animator aminator;
        private Level level; 
        private Hero hero;
        
        public Game Game {
            set {
                level = value.Level;
                hero = value.Hero;
            }
        }

        protected override void Start() {
            aminator = GetComponent<Animator>();
            base.Start();
    	}
    	
        protected override void Update() {
            aminator.SetBool(IS_DEAD_KEY, IsDead());
            base.Update();
    	}

        public bool IsSync() {
            return !IsMoving();    
        }

        private bool IsDead() {
            return hero != null && hero.IsDead;
        }

        protected override Vector3 GetNewPosition() {
            return GameUtils.ConvertToGameCoord(hero.X, hero.Y, level);
        }
    }
}
