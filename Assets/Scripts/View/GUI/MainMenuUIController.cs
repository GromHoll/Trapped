using UnityEngine;
using System.Collections;

namespace TrappedGame.View.GUI {
    public class MainMenuUIController : MonoBehaviour {

        // Really dirty hack, but while I don't know better way
        static GameObject mainMenu = null;

        void Awake() {
            DontDestroyOnLoad(transform.gameObject); 
            if (mainMenu != null) {
                GameObject.Destroy(transform.gameObject);
                mainMenu.SetActive(true);
            } else {
                mainMenu = transform.gameObject;
            }
        }

    }
}
