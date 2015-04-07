using System.Globalization;
using TrappedGame.Model;
using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
    public class WinMenu : MonoBehaviour {

        public Text scoreField;
        public Text deathField;
        public AudioClip winAudio;

        private bool isShowed = false;

        public void Show(Game game) {
            if (!isShowed) {
                isShowed = true;
                gameObject.SetActive(true);
                scoreField.text = game.GetScore().ToString(CultureInfo.InvariantCulture);
                deathField.text = game.Hero.DeathCount.ToString(CultureInfo.InvariantCulture);
                AudioSource.PlayClipAtPoint(winAudio, Vector3.zero);
            }
        }

        public void ReturnToMenu() {
            Application.LoadLevel("MainMenu");
        }
    }
}
