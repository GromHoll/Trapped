using System;

namespace TrappedGame {   
    public interface ITickable {
        void NextTick();
        void BackTick();
    }
}