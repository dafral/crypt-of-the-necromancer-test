using UnityEngine;

namespace Dafral.Game
{
    [CreateAssetMenu(fileName = "NewGameConfiguration", menuName = "Dafral/Game/Game Configuration")]
    public class GameConfiguration : ScriptableObject
    {
        [SerializeField] private RhythmController _rhythmController;
        [SerializeField] private LevelConfiguration[] _levels;

        public RhythmController RhythmController => _rhythmController;
        public LevelConfiguration[] Levels => _levels;
    }
}
