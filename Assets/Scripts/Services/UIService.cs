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
    }
}
