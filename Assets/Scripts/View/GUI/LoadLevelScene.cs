using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
	public class LoadLevelScene : MonoBehaviour {
		void Start() {
			gameObject.GetComponent<Button>().onClick.AddListener(() => LoadScene("Level"));
		}
		
		public void LoadScene(string sceneName) {
			Application.LoadLevel(sceneName);	                       
		}
	}
}