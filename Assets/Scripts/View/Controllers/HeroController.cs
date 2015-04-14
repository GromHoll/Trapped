using System.Collections.Generic;
using TrappedGame.Model;
using TrappedGame.Utils;
using TrappedGame.Utils.Observer;
using TrappedGame.View.Controllers.Common;
using TrappedGame.View.Graphic;
using TrappedGame.View.Sync;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class HeroController : MovableObjectController, ISyncGameObject, IObserver<Path.PathLink> {
        
        public static readonly string IS_DEAD_KEY = "IsDead";

        public PathGOFactory pathFactory;

        public Hero Hero { get; private set; }
        public Level Level { get; private set; }

        private CameraController cameraController;
        private Animator animator;

        private readonly Queue<Path.PathLink> movementsQueue = new Queue<Path.PathLink>();
        
        public Game Game {
            set {
                Level = value.Level;
                Hero = value.Hero;
                Hero.MovementSubject.AddObserver(this);
            }
        }

        protected override void Start() {
            animator = GetComponent<Animator>();
            cameraController = Camera.main.gameObject.GetComponent<CameraController>();
            base.Start();
    	}
    	
        protected override void Update() {
            animator.SetBool(IS_DEAD_KEY, IsDead());
            cameraController.SetDead(IsDead());
            base.Update();
    	}

        public bool IsSync() {
            return !IsMoving();    
        }

        private bool IsDead() {
            return Hero != null && Hero.IsDead;
        }

        protected override Vector3 GetNewPosition() {
            while (true) {
                if (movementsQueue.Count == 0) {
                    return gameObject.transform.position;
                }
                var link = movementsQueue.Dequeue();

                if (link.IsAdjacent()) {
                    if (Hero.Contains(link)) {
                        pathFactory.CreateLink(link, this);
                    }
                    return GameUtils.ConvertToGameCoord(link.ToX, link.ToY, Level);
                }
                gameObject.transform.position = GameUtils.ConvertToGameCoord(link.ToX, link.ToY, Level);
            }
        }

        public void Update(Path.PathLink message) {
            movementsQueue.Enqueue(message);
        }
    }
}
