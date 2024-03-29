using System;

namespace TrappedGame.Model.Cells {
    public class CountCell : Cell {

        public static readonly int DEFAULT_PERIOD = 1;
        public static readonly bool DEFAULT_ON_STATE = false;

        public bool IsOn { get; protected set; }
        public int OnPeriod { get; protected set; }
        public int OffPeriod { get; protected set; }
        public int CurrentTick { get; protected set; }

        public CountCell(int x, int y, int onPeriod, int offPeriod, int currentTick, bool isOn)
            : base(x, y) {        
            if (currentTick >= (isOn ? onPeriod : offPeriod)) throw new ArgumentException("CurrentTick can't be more than period"); 
            if (onPeriod < 1) throw new ArgumentException("OnPeriod should be more than zero"); 
            if (offPeriod < 1) throw new ArgumentException("OffPeriod should be more than zero");

            IsOn = isOn;
            OnPeriod = onPeriod;
            OffPeriod = offPeriod;
            CurrentTick = currentTick;
        }

        public CountCell(int x, int y)
            : this(x, y, DEFAULT_PERIOD, DEFAULT_PERIOD, 0, DEFAULT_ON_STATE) {}

        public override void NextTick() {
            CurrentTick++;
            if (CurrentTick >= GetCurrentPeriod()) {
                CurrentTick = 0;
                IsOn = !IsOn;
            }
        }

        public override void BackTick() {
            CurrentTick--;
            if (CurrentTick == -1) {
                CurrentTick = GetOtherPeriod() - 1;
                IsOn = !IsOn;
            }
        }
        
        public bool IsOnOnNextTick() {
            return (IsOn && CurrentTick < OnPeriod - 1) || (!IsOn && CurrentTick + 1 == OffPeriod);
        }

        public bool IsOnAfter(int tickCount) {
            var ticks = tickCount % (OnPeriod + OffPeriod);
            if (IsOn && ticks < (OnPeriod - CurrentTick)) {
                return true;
            } else if (!IsOn && ticks < (OffPeriod - CurrentTick)) {
                return false;
            }
            return !IsOn;
        }

        private int GetCurrentPeriod() {
            return IsOn ? OnPeriod : OffPeriod;
        }

        private int GetOtherPeriod() {
            return IsOn ? OffPeriod : OnPeriod;
        }
    }
}
