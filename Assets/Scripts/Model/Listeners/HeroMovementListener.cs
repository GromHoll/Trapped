using System;
using TrappedGame.Model.Game;

namespace TrappedGame {
    public interface HeroMovementListener {
        void HeroMoved(Hero hero);
    }
}

