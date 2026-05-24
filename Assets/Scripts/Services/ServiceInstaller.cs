using Dafral.Game.Map;

namespace Dafral.Services
{
    public class ServiceInstaller
    {
        public void Install(UIConfiguration uiConfiguration)
        {
            var eventService = ServiceLocator.Instance.RegisterService<IEventService>(new EventService());
            ServiceLocator.Instance.RegisterService<IInputService>(new InputService(eventService));
            ServiceLocator.Instance.RegisterService<IUIService>(new UIService(uiConfiguration));
            ServiceLocator.Instance.RegisterService<IGridService>(new GridService());
        }
    }
}
