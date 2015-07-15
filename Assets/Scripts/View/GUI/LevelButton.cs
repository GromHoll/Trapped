using TrappedGame.Main;
using TrappedGame.Model.LevelUtils;
using TrappedGame.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace TrappedGame.View.GUI {
	public class LevelButton : MonoBehaviour {

        public Color notStartedColor;
        public Color startedColor;
        public Color finishedColor;

        public Levels.LevelInfo Level { get; set; }
        
	    void Update() {
            if (Level == null) { return; }
            Button button = GetComponent<Button>();
            button.image.color = getColor();
	    }

        private Color getColor() {
            switch (Level.State) {
                case Levels.LevelInfo.LEVEL_STATE.NOT_STARTED:
                    return notStartedColor;
                case Levels.LevelInfo.LEVEL_STATE.STARTED:
                    return startedColor;
                case Levels.LevelInfo.LEVEL_STATE.FINISHED:
                    return finishedColor;
            }
            return Color.white;
        }

	}
}
