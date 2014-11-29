using System;
using TrappedGame.Model;

namespace TrappedGame {
    public interface HeroMovementListener {
        void HeroMoved(Hero hero);
    }
}

