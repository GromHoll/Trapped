using TrappedGame.Model;
using TrappedGame.View.Sync;
using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
    public class TutorialMenu : MonoBehaviour, ISyncGameObject {
        
        public Text tutorialName;
        public Text tutorialMessage;
		
        public void ShowTutorial(Tutorial tutorial) {
            if (tutorial == null) { return; }

            gameObject.SetActive(true);
            tutorialName.text = tutorial.Name;
            tutorialMessage.text = tutorial.Message;
        }

        public bool IsSync() {
            return !gameObject.activeSelf;
        }

        public void Update() {
            if (Input.anyKey) {
                gameObject.SetActive(false);
            }
        }
    }
}
