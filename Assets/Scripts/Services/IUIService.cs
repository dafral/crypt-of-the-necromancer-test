using Dafral.Game;

namespace Dafral.Services
{
    public interface IUIService
    {
        void CreateRhythmBar(RhythmController rhythmController);
        void CreatePlayerHealthBar();
        void ShowGameOverScreen(bool isVictory);
        void ShowStartGameScreen();
        void ShowScreenControls();
    }
}
