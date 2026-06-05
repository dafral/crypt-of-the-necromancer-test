using Dafral.Game.UI;
using UnityEngine;

namespace Dafral.Services
{
    [CreateAssetMenu(fileName = "NewUIConfiguration", menuName = "Dafral/UI/UI Configuration")]
    public class UIConfiguration : ScriptableObject
    {
        public Canvas CanvasPrefab;
        public RhythmBarController RhythmBarPrefab;
        public PlayerHealthBarController PlayerHealthBarPrefab;
        public GameOverScreenController GameOverScreenPrefab;
        public StartGameScreenController StartGameScreenPrefab;
        public ScreenControlsController ScreenControlsPrefab;
    }
}
