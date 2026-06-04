using Dafral.Game.Map;
using UnityEngine;

namespace Dafral.Game
{
    [CreateAssetMenu(fileName = "NewLevelConfiguration", menuName = "Dafral/Game/Level Configuration")]
    public class LevelConfiguration : ScriptableObject
    {
        [SerializeField] private LevelData _levelData;
        [SerializeField] private MapConfiguration _mapConfiguration;

        public LevelData LevelData => _levelData;
        public MapConfiguration MapConfiguration => _mapConfiguration;

        public void SetLevelData(LevelData levelData)
        {
            _levelData = levelData;
        }
    }
}
