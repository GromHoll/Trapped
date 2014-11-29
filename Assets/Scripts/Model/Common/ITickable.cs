namespace TrappedGame.Model.Common {   
    public interface ITickable {
        void NextTick();
        void BackTick();
    }
}