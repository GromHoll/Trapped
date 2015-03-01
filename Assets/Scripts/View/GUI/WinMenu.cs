using System.Globalization;
using TrappedGame.Model;
using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
    public class WinMenu : MonoBehaviour {

        public Text scoreField;
        public Text deathField;

        public void Show(Game game) {
            gameObject.SetActive(true);
            scoreField.text = game.GetScore().ToString(CultureInfo.InvariantCulture);
            deathField.text = game.Hero.DeathCount.ToString(CultureInfo.InvariantCulture);
        }

        public void ReturnToMenu() {
            Application.LoadLevel("MainMenu");
        }
    }
}
