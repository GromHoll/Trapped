using TrappedGame.Model.Game;
using UnityEngine;

namespace TrappedGame {
    public class WinMenu : MonoBehaviour {

        private Game game;

    	void OnGUI() {
            GUI.Box (new Rect(10, 10, 100, 150), "WIN!\n Score = " + game.GetScore()
                     + "\nYou die " + game.GetHero().GetDeaths() + " count");
    		
    		if (GUI.Button(new Rect(20, 80, 80, 20), "Back!")) {
    			Application.LoadLevel("MainMenu");
    		}
    	}

        public void SetGame(Game game) {
            this.game = game;
        }

        public void SetActive(bool isShow) {
            gameObject.SetActive(isShow);
        }
    }
}
