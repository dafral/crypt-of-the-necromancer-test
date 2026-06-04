namespace Dafral.Services
{
    public interface IGameService
    {
        void StartGame();
        void RestartGame();
        void RestartCurrentLevel();
        void BeatLevel();
    }
}
