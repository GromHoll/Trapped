using TrappedGame.Model;
using TrappedGame.Model.Listeners;
using TrappedGame.Utils;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class HeroController : MonoBehaviour, IHeroMovementListener {
        
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

        public void SetGame(Game game) {
            level = game.GetLevel();
            hero = game.GetHero();
            game.AddHeroMovementListener(this);
        }

        public bool IsMoving() {
            return targetPosition != gameObject.transform.position;
        }

        private bool IsDead() {
            return hero.IsDead();
        }

        private void UpdatePosition() {
            if (IsMoving()) {
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
        
        public void HeroMoved(Hero eventHero) {
            targetPosition = GameUtils.ConvertToGameCoord(eventHero.GetX(), eventHero.GetY(), level);
        }

    }
}
