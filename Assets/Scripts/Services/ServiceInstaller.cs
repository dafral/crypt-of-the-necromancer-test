using Dafral.Grid;

namespace Dafral.Services
{
    public class ServiceInstaller
    {
        public void Install()
        {
            var eventService = ServiceLocator.Instance.RegisterService<IEventService>(new EventService());
            ServiceLocator.Instance.RegisterService<IInputService>(new InputService(eventService));
            ServiceLocator.Instance.RegisterService<IGridService>(new GridService());
        }
    }
}
