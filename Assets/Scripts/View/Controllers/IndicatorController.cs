﻿using TrappedGame.Model.Cells;
using UnityEngine;

namespace TrappedGame.View.Controllers {
    public class IndicatorController : MonoBehaviour {
        public bool state;
        public CountCell Cell { get; set; }

        private void Update() {
            renderer.enabled = Cell.IsOnOnNextTick() == state;
        }
    }
}
