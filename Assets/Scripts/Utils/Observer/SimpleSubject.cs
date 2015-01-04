using System.Collections.Generic;

namespace TrappedGame.Utils.Observer {
    public class SimpleSubject<T> : ISubject<T> {

        private readonly IList<IObserver<T>> observers = new List<IObserver<T>>();

        public void AddObserver(IObserver<T> observer) {
            if (observer != null && !observers.Contains(observer)) {
                observers.Add(observer);
            }
        }

        public void RemoveObserver(IObserver<T> observer) {
            observers.Remove(observer);
        }

        public void NotifyObservers(T message) {
            foreach (var observer in observers) {
                observer.Update(message);
            }
        }
    }
}
