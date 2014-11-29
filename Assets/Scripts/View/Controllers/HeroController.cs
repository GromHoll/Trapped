using TrappedGame.Model;
using UnityEngine;
using System.Collections;

namespace TrappedGame {
    public class HeroController : MonoBehaviour, HeroMovementListener {
        
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
            this.level = game.GetLevel();
            this.hero = game.GetHero();
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
                Vector3 currentPosition = gameObject.transform.position;
                Vector3 direction = targetPosition - currentPosition;
                direction.Normalize();
                direction *= speed*Time.deltaTime; 

                Vector3 newPosition = gameObject.transform.position + direction;                
                if (Vector3.Distance(targetPosition, newPosition) >= Vector3.Distance(targetPosition, currentPosition)) {
                    newPosition = targetPosition;
                }
                gameObject.transform.position = newPosition;
            }
        }
        
        public void HeroMoved(Hero hero) {
            targetPosition = GameUtils.ConvertToGameCoord(hero.GetX(), hero.GetY(), level);
        }

    }
}
