using System;

namespace TrappedGame.Model.Cells {
    public class CountCell : Cell {

        public static readonly int DEFAULT_PERIOD = 1;
        public static readonly bool DEFAULT_ON_STATE = false;

        protected bool isOn;
        protected int onPeriod;
        protected int offPeriod;
        public int currentTick;

        public CountCell(int x, int y, CellType type, int onPeriod, int offPeriod, int currentTick, bool isOn)
            : base(x, y, type) {        
            if (currentTick >= (isOn ? onPeriod : offPeriod)) throw new ArgumentException("CurrentTick can't be more than period"); 
            if (onPeriod < 1) throw new ArgumentException("OnPeriod should be more than zero"); 
            if (offPeriod < 1) throw new ArgumentException("OffPeriod should be more than zero"); 

            this.onPeriod = onPeriod;
            this.offPeriod = offPeriod;
            this.currentTick = currentTick;
            this.isOn = isOn;
        }

        public CountCell(int x, int y, CellType type)
            : this(x, y, type, DEFAULT_PERIOD, DEFAULT_PERIOD, 0, DEFAULT_ON_STATE) {}

        public override void NextTick() {
            currentTick++;
            if (currentTick >= GetCurrentPeriod()) {
                currentTick = 0;
                isOn = !isOn;
            }
        }

        public override void BackTick() {
            currentTick--;
            if (currentTick == -1) {
                currentTick = GetOtherPeriod() - 1;
                isOn = !isOn;
            }
        }

        public bool IsOn() {
            return isOn;
        }

        public bool IsOnOnNextTick() {
            return (isOn && currentTick < onPeriod - 1) || (!isOn && currentTick + 1 == offPeriod);
        }

        private int GetCurrentPeriod() {
            return isOn ? onPeriod : offPeriod;
        }

        private int GetOtherPeriod() {
            return isOn ? offPeriod : onPeriod;
        }
    }
}
