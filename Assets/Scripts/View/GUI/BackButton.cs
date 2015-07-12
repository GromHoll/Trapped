using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace TrappedGame.View.GUI {
    public class BackButton : MonoBehaviour {

        void Update() {
            bool isEscaped = Input.GetKeyDown(KeyCode.Escape);
            if (isEscaped) {
                //ExecuteEvents.Execute<IPointerClickHandler>(
                //    ExecuteEvents.GetEventHandler<IPointerClickHandler>(gameObject), 
                //    new PointerEventData(EventSystem.current), 
                //    ExecuteEvents.pointerClickHandler);

                ExecuteEvents.Execute<IPointerClickHandler>(
                    gameObject, 
                    new PointerEventData(EventSystem.current), 
                    ExecuteEvents.pointerClickHandler);
            }
        }
    }
}
