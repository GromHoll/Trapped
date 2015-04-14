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
            UpdateLineScale();
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

        private void UpdateLineScale() {
            var heroCoord = HeroController.transform.position;
            var linkCoord = transform.position;

            var deviation = PathLink.IsVertical() ? heroCoord.x - linkCoord.x : heroCoord.y - linkCoord.y;

            var localScale = line.transform.localScale;
            if (deviation == 0) {
                var scale = PathLink.IsHorizontal() ? heroCoord.x - linkCoord.x : heroCoord.y - linkCoord.y;
                localScale.x = Mathf.Min(Mathf.Abs(scale), 1);
            } else {
                localScale.x = 1;
            }
            line.transform.localScale = localScale;
        }

		void Update() {
            if (HeroController.transform.position == transform.position) {
                if (!HeroController.Hero.Contains(PathLink)) {
                    GameObject.Destroy(gameObject);
                }
            } else {
                UpdateLineScale();
            }
		}



	}
}
