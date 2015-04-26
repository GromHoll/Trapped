using TrappedGame.Model.Cells;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    class Indicator4Controller : MonoBehaviour {

        public Color safeColor = Color.green;
        public Color dangerColor = Color.red;

        //  |  LU | RU  ^
        //  |  ---+---  |
        //  |  LD | RD  |
        //  V---------->|
        public SpriteRenderer LeftUpSprite;
        public SpriteRenderer LeftDownSprite;
        public SpriteRenderer RightDownSprite;
        public SpriteRenderer RightUpSprite;

        public CountCell Cell { get; set; }

        void Update() {
            LeftUpSprite.color    = Cell.IsOnAfter(0) ? dangerColor : safeColor;
            LeftDownSprite.color  = Cell.IsOnAfter(1) ? dangerColor : safeColor;
            RightDownSprite.color = Cell.IsOnAfter(2) ? dangerColor : safeColor;
            RightUpSprite.color   = Cell.IsOnAfter(3) ? dangerColor : safeColor;
        }

    }
}
