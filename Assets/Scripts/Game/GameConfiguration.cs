using UnityEngine;

namespace Dafral.Game
{
    [CreateAssetMenu(fileName = "NewGameConfiguration", menuName = "Dafral/Game/Game Configuration")]
    public class GameConfiguration : ScriptableObject
    {
        [SerializeField] private RhythmController _rhythmController;
        [SerializeField] private AudioClip _rhythmBeatSound;
        [SerializeField] private LevelConfiguration[] _levels;

        public RhythmController RhythmController => _rhythmController;
        public AudioClip RhythmBeatSound => _rhythmBeatSound;
        public LevelConfiguration[] Levels => _levels;
    }
}
