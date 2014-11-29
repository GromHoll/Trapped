using System;
using TrappedGame.Model;

namespace TrappedGame {
    public class LevelTick {
        
        private int tickCount = 1;

        public LevelTick(int tickCount) {
            if (tickCount < 0) throw new ArgumentException("Tick Count should be positive", "tickCount");
            this.tickCount = tickCount;
        }

        public void BackTick(Level level) {
            for (int i = 0; i < tickCount; i++) {
                level.BackTick();
            }
        }

        public void NextTick(Level level) {
            for (int i = 0; i < tickCount; i++) {
                level.NextTick();
            }
        }
        
        public static readonly LevelTick FREEZE_LEVEL_TICK = new LevelTick(0);
        public static readonly LevelTick DEFAULT_LEVEL_TICK = new LevelTick(1);
    }
}