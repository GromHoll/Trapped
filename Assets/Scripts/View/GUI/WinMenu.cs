using TrappedGame.Model;
using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
    public class WinMenu : MonoBehaviour {

        private Game game;

		public GameObject scoreField;
		public GameObject deathField;
		

        public void SetGame(Game newGame) {
            game = newGame;
        }

		public void Hide() {
			gameObject.SetActive(false);
		}

        public void Show() {
            gameObject.SetActive(true);

			scoreField.GetComponent<Text>().text = game.GetScore().ToString();
			deathField.GetComponent<Text>().text = game.Hero.DeathCount.ToString();
        }

		public void ReturnToMenu() {
			Application.LoadLevel("MainMenu");
		}
    }
}
