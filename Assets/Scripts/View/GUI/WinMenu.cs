using TrappedGame.Model;
using UnityEngine;

namespace TrappedGame.View.GUI {
    public class WinMenu : MonoBehaviour {

        private Game game;

    	void OnGUI() {
            UnityEngine.GUI.Box (new Rect(10, 10, 100, 150), "WIN!\n Score = " + game.GetScore()
                     + "\nYou die " + game.GetHero().GetDeaths() + " count");
    		
    		if (UnityEngine.GUI.Button(new Rect(20, 80, 80, 20), "Back!")) {
				ReturnToMenu();
    		}
    	}

        public void SetGame(Game newGame) {
            game = newGame;
        }

        public void SetActive(bool isShow) {
            gameObject.SetActive(isShow);
        }

		public void ReturnToMenu() {
			Application.LoadLevel("MainMenu");
		}
    }
}
