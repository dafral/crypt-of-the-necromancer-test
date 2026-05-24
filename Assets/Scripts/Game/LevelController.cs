using Dafral.Game.Map;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private MapConfiguration _mapConfiguration;

        private void Start()
        {
            InitializeLevel();
        }

        private void InitializeLevel()
        {
            ServiceLocator.Instance.GetService<IGridService>().LoadMap(_mapConfiguration);
        }

        private void SpawnPlayer()
        {

        }
    }
}