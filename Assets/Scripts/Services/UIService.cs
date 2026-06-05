using Dafral.Game;
using Dafral.Game.UI;
using UnityEngine;

namespace Dafral.Services
{
    public class UIService : IUIService
    {
        private Canvas _canvas;
        private UIConfiguration _uiConfiguration;

        public UIService(UIConfiguration uiConfiguration)
        {
            _uiConfiguration = uiConfiguration;
            CreateCanvas();
        }

        private void CreateCanvas()
        {
            _canvas = Object.Instantiate(_uiConfiguration.CanvasPrefab);
        }

        public void CreateRhythmBar(RhythmController rhythmController)
        {
            RhythmBarController rhythmBarController = Object.Instantiate(_uiConfiguration.RhythmBarPrefab, _canvas.transform);
            rhythmBarController.Initialize(rhythmController);
        }

        public void CreatePlayerHealthBar()
        {
            PlayerHealthBarController healthBarController = Object.Instantiate(_uiConfiguration.PlayerHealthBarPrefab, _canvas.transform);
            healthBarController.Initialize();
        }

        public void ShowGameOverScreen(bool isVictory)
        {
            GameOverScreenController gameOverScreenController = Object.Instantiate(_uiConfiguration.GameOverScreenPrefab, _canvas.transform);
            gameOverScreenController.Initialize(isVictory);
        }

        public void ShowStartGameScreen()
        {
            StartGameScreenController startGameScreenController = Object.Instantiate(_uiConfiguration.StartGameScreenPrefab, _canvas.transform);
            startGameScreenController.Initialize();
        }

        public void ShowScreenControls()
        {
            ScreenControlsController screenControlsController = Object.Instantiate(_uiConfiguration.ScreenControlsPrefab, _canvas.transform);
            screenControlsController.Initialize();
        }
    }
}
