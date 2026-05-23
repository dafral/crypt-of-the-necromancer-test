using Dafral.Events;

namespace Dafral.Services
{
    public interface IEventService
    {
        void Subscribe<T>(EventDelegate<T> del) where T : IEvent;
        void Unsubscribe<T>(EventDelegate<T> del) where T : IEvent;
        void RaiseEvent(IEvent e);
    }
}
