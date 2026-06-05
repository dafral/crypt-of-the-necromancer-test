using Dafral.Enemies;
using Dafral.Game;
using Dafral.Game.Map;
using Dafral.Player;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Bootstrapper
{
    public class Installer : MonoBehaviour
    {
        [SerializeField] private PlayerConfiguration _playerConfiguration;
        [SerializeField] private EnemyLibrary _enemyLibrary;
        [SerializeField] private UIConfiguration _uiConfiguration;
        [SerializeField] private GameConfiguration _gameConfiguration;

        private void Awake()
        {
            InitializeServices();
            InitializeGame();
        }

        private void InitializeServices()
        {
            var serviceLocator = ServiceLocator.Instance;
            var eventService = serviceLocator.RegisterService<IEventService>(new EventService());
            serviceLocator.RegisterService<IInputService>(new InputService(eventService));
            var uiService = serviceLocator.RegisterService<IUIService>(new UIService(_uiConfiguration));
            var gridService = serviceLocator.RegisterService<IGridService>(new GridService(eventService));
            serviceLocator.RegisterService<IEntityService>(new EntityService(_playerConfiguration, _enemyLibrary));
            serviceLocator.RegisterService<IGameService>(new GameService(_gameConfiguration, gridService, uiService, eventService));
        }

        private void InitializeGame()
        {
            ServiceLocator.Instance.GetService<IGameService>().StartGame();
        }
    }
}
