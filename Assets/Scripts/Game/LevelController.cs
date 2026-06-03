using Dafral.Game.Map;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game
{
    public class LevelController : MonoBehaviour
    {
        [SerializeField] private LevelConfiguration _levelConfiguration;
        [SerializeField] private RhythmController _rhythmController;

        private void Start()
        {
            CreatePlayerHealthBar();
            CreateMap();
            InitializeRhythm();
        }

        private void CreatePlayerHealthBar()
        {
            ServiceLocator.Instance.GetService<IUIService>().CreatePlayerHealthBar();
        }

        private void CreateMap()
        {
            ServiceLocator.Instance.GetService<IGridService>().LoadMap(_levelConfiguration.MapConfiguration);
        }

        private void InitializeRhythm()
        {
            _rhythmController.Initialize(_levelConfiguration.LevelData.RhythmTempo);
        }
    }
}