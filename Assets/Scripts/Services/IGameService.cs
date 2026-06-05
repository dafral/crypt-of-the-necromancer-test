using Dafral.Game;

namespace Dafral.Services
{
    public interface IGameService
    {
        IRhythmController RhythmController { get; }
        void StartGame();
        void RestartGame();
        void RestartCurrentLevel();
        void BeatLevel();
    }
}
