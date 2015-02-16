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
            camera.backgroundColor = defautColor;
        }

        private void Update() {
            if (camera.backgroundColor != targetColor) {
                camera.backgroundColor = Color.Lerp(camera.backgroundColor, targetColor, Time.deltaTime*changeSpeed);
            }
        }

        public void SetDead(bool isDead) {
            targetColor = isDead ? deathColor : defautColor;
        }

    }
}
