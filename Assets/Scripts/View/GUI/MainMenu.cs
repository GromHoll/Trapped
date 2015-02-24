using UnityEngine;

namespace TrappedGame.View.GUI {
	public class MainMenu : MonoBehaviour {




        public void Quit() {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
			    Application.Quit();
            #endif
        }
	}
}
