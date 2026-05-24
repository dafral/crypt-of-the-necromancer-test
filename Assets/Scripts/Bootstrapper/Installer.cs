using Dafral.Enemies;
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

        private ServiceInstaller _serviceInstaller = new();

        private void Awake()
        {
            InitializeServices();
            InitializeFactories();
        }

        private void InitializeServices()
        {
            _serviceInstaller.Install(_uiConfiguration);
        }

        private void InitializeFactories()
        {
            PlayerFactory.Initialize(_playerConfiguration);
            EnemyFactory.Initialize(_enemyLibrary);
        }
    }
}
