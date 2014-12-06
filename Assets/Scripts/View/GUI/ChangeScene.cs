using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
	public class ChangeScene : MonoBehaviour {
		void Start() {
			gameObject.GetComponent<Button>().onClick.AddListener(() => LoadScene("MainMenu"));
		}

		public void LoadScene(string sceneName) {
			Application.LoadLevel(sceneName);	                       
		}
	}
}