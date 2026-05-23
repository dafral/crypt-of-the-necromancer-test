using Dafral.Services;
using UnityEngine;

namespace Dafral.Bootstrapper
{
    public class Installer : MonoBehaviour
    {
        private ServiceInstaller _serviceInstaller = new();

        private void Awake()
        {
            InitializeServices();
        }

        private void InitializeServices()
        {
            _serviceInstaller.Install();
        }
    }
}
