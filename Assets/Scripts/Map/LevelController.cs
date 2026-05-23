using Dafral.Grid;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Level
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private LevelDataConfiguration _levelDataConfiguration;

        private void Start()
        {
            InitializeLevel();
        }

        private void InitializeLevel()
        {
            ServiceLocator.Instance.GetService<IGridService>().LoadLevel(_levelDataConfiguration);
        }

        private void SpawnPlayer()
        {
            
        }
    }
}