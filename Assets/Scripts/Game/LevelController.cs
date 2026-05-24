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
            CreateMap();
            InitializeRhythm();
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