namespace TrappedGame.Utils.Observer {
    public interface IObserver<in T> {
        void Update(T message);
    }
}
