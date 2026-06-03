using Dafral.Enemies;
using Dafral.Game;
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

        private ServiceInstaller _serviceInstaller = new();

        private void Awake()
        {
            InitializeServices();
            InitializeFactories();
            InitializeGame();
        }

        private void InitializeServices()
        {
            _serviceInstaller.Install(_uiConfiguration, _gameConfiguration);
        }

        private void InitializeFactories()
        {
            PlayerFactory.Initialize(_playerConfiguration);
            EnemyFactory.Initialize(_enemyLibrary);
        }

        private void InitializeGame()
        {
            ServiceLocator.Instance.GetService<IGameService>().StartGame();
        }
    }
}
