using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
	public class QuitGame : MonoBehaviour {
		void Start() {
			gameObject.GetComponent<Button>().onClick.AddListener(() => Quit());
		}

		public void Quit() {
			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}
	}
}
