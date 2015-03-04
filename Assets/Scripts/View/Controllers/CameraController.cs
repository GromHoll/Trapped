using TrappedGame.Model.Cells;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class CameraController : MonoBehaviour {

        public Color defautColor;
        public Color deathColor;
        public float changeSpeed = 1;

        private Color targetColor;

        private void Start() {
            targetColor = defautColor;
            GetComponent<Camera>().backgroundColor = defautColor;
        }

        private void Update() {
            if (GetComponent<Camera>().backgroundColor != targetColor) {
                GetComponent<Camera>().backgroundColor = Color.Lerp(GetComponent<Camera>().backgroundColor, targetColor, Time.deltaTime*changeSpeed);
            }
        }

        public void SetDead(bool isDead) {
            targetColor = isDead ? deathColor : defautColor;
        }

    }
}
