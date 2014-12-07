using TrappedGame.Model.Cells;
using UnityEngine;
using System.Collections;


namespace TrappedGame.View.Controllers {
    public class LaserLineController : MonoBehaviour {

        private LaserCell.Line line;
        private float lenght;

        private void Start() {
            lenght = GetLenght();
            ScaleLaser();
            MoveLaser();
            RotateLaser();
        }

        private void Update() {
            foreach (Transform child in transform) {
                child.gameObject.SetActive(line.IsDanger());
            }
        }

        public void SetLaserLine(LaserCell.Line newLine) {
            line = newLine;
        }

        private float GetLenght() {
            var cover = line.Cover;
            if (line.IsVertical()) {
                return cover.MaxY - cover.MinY + 1;
            }
            return cover.MaxX - cover.MinX + 1;
        }

        private float GetSafeShift() {
            return lenght == 1 ? 0 : (lenght - 1) / 2;
        }

        private void ScaleLaser() {
            var localScaleX = gameObject.transform.localScale.x;
            gameObject.transform.localScale = new Vector3(lenght * localScaleX, 1, 1);
        }

        private void MoveLaser() {
            gameObject.transform.position += new Vector3(
                    line.IsHorizontal() ? GetSafeShift() : 0,
                    line.IsVertical() ? GetSafeShift() : 0,
                    0);
        }

        private void RotateLaser() {
            if (line.IsVertical()) {
                gameObject.transform.Rotate(0, 0, 90);
            }
        }
    }
}
