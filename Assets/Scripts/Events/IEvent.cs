namespace Dafral.Events
{
    public interface IEvent
    {

    }

    public delegate void EventDelegate<T>(T e) where T : IEvent;
}