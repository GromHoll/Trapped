using System.Collections;
using TrappedGame.Model;
using UnityEngine;

namespace TrappedGame.View.Controllers {
	public class PathLinkController : MonoBehaviour {

        public bool crossRotation = true;
		public GameObject cross;
		public GameObject line;

        public Path.PathLink PathLink { get; set; }
        public HeroController HeroController { get; set; }

		void Start() {
            transform.Rotate(0, 0, GetRotation());
            if (crossRotation) {
                cross.transform.Rotate(0, 0, GetCrossRotation());
            }
		}

        private float GetRotation() {
            if (PathLink.IsWentDown()) { return 270; }
            if (PathLink.IsWentLeft()) { return 180; }
            if (PathLink.IsWentUp()) { return 90; }
            return 0;   /* Else IsWentRight */
        }

        private float GetCrossRotation() {
            var previous = PathLink.PreviousLink;
            if (previous == null) { return 0; }

            if ((previous.IsWentUp() && PathLink.IsWentLeft()) ||
                (previous.IsWentRight() && PathLink.IsWentUp()) ||
                (previous.IsWentDown() && PathLink.IsWentRight()) ||
                (previous.IsWentLeft() && PathLink.IsWentDown())) {
                return 90;
            }
            return 0;
        }

		void Update() {
            if (HeroController.transform.position == transform.position) {
                if (!HeroController.Hero.WasHere(PathLink.From)) {
                    GameObject.Destroy(gameObject);
                }
            } else {
                // TODO Portal links not destroys
            }
		}

	}
}
