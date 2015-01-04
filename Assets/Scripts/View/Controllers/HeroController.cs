using System.Collections.Generic;
using TrappedGame.Model;
using TrappedGame.Utils;
using TrappedGame.Utils.Observer;
using TrappedGame.View.Controllers.Common;
using TrappedGame.View.Sync;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class HeroController : MovableObjectController, ISyncGameObject, IObserver<Path.PathLink> {
        
        public static readonly string IS_DEAD_KEY = "IsDead";
        
        private Animator aminator;
        private Level level; 
        private Hero hero;

        private readonly Queue<Path.PathLink> movementsQueue = new Queue<Path.PathLink>();
        
        public Game Game {
            set {
                level = value.Level;
                hero = value.Hero;
                hero.MovementSubject.AddObserver(this);
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
            while (true) {
                if (movementsQueue.Count == 0) {
                    return gameObject.transform.position;
                }
                var link = movementsQueue.Dequeue();
                if (link.IsAdjacent()) {
                    return GameUtils.ConvertToGameCoord(link.ToX, link.ToY, level);
                }
                gameObject.transform.position = GameUtils.ConvertToGameCoord(link.ToX, link.ToY, level);
            }
        }

        public void Update(Path.PathLink message) {
            movementsQueue.Enqueue(message);
        }
    }
}
