using Dafral.Player;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Bootstrapper
{
    public class Installer : MonoBehaviour
    {
        [SerializeField] private PlayerConfiguration _playerConfiguration;
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
        }
    }
}
