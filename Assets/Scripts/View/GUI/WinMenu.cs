using System.Globalization;
using TrappedGame.Model;
using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
    public class WinMenu : MonoBehaviour {

        private Game game;

        public Text scoreField;
        public Text deathField;

        public void SetGame(Game newGame) {
            game = newGame;
        }

		public void Hide() {
			gameObject.SetActive(false);
		}

        public void Show() {
            gameObject.SetActive(true);
			scoreField.text = game.GetScore().ToString(CultureInfo.InvariantCulture);
			deathField.text = game.Hero.DeathCount.ToString(CultureInfo.InvariantCulture);
        }

		public void ReturnToMenu() {
			Application.LoadLevel("MainMenu");
		}
    }
}
