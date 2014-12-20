using TrappedGame.Model;
using TrappedGame.Model.Listeners;
using TrappedGame.Utils;
using TrappedGame.View.Sync;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class HeroController : MonoBehaviour, IHeroMovementListener, ISyncGameObject {
        
        public static readonly string IS_DEAD_KEY = "IsDead";
        
        private Animator aminator;
        private Level level; 
        private Hero hero;

        private Vector3 targetPosition;
        public float speed = 10;

        void Start() {
            aminator = GetComponent<Animator>();
            targetPosition = gameObject.transform.position;
    	}
    	
        void Update() {
            aminator.SetBool(IS_DEAD_KEY, IsDead());
            UpdatePosition();
    	}

        public Game Game {
            set {
                level = value.Level;
                hero = value.Hero;
                value.AddHeroMovementListener(this);
            }
        }

        public bool IsSync() {
            return targetPosition == gameObject.transform.position;    
        }

        private bool IsDead() {
            return hero != null && hero.IsDead;
        }

        private void UpdatePosition() {
            if (!IsSync()) {
                var currentPosition = gameObject.transform.position;
                var direction = targetPosition - currentPosition;
                direction.Normalize();
                direction *= speed*Time.deltaTime; 

                var newPosition = gameObject.transform.position + direction;                
                if (Vector3.Distance(targetPosition, newPosition) >= Vector3.Distance(targetPosition, currentPosition)) {
                    newPosition = targetPosition;
                }
                gameObject.transform.position = newPosition;
            }
        }
        
        // TODO rewrite without listeners
        public void HeroMoved(Hero eventHero) {
            targetPosition = GameUtils.ConvertToGameCoord(eventHero.X, eventHero.Y, level);
        }

    }
}
