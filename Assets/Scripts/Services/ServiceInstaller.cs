using Dafral.Game;
using Dafral.Game.Map;

namespace Dafral.Services
{
    public class ServiceInstaller
    {
        public void Install(UIConfiguration uiConfiguration, GameConfiguration gameConfiguration)
        {
            var eventService = ServiceLocator.Instance.RegisterService<IEventService>(new EventService());
            ServiceLocator.Instance.RegisterService<IInputService>(new InputService(eventService));
            var uiService = ServiceLocator.Instance.RegisterService<IUIService>(new UIService(uiConfiguration));
            var gridService = ServiceLocator.Instance.RegisterService<IGridService>(new GridService(eventService));
            ServiceLocator.Instance.RegisterService<IGameService>(new GameService(gameConfiguration, gridService, uiService, eventService));
        }
    }
}
