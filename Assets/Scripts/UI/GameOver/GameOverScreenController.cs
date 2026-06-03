using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.UI
{
    public class GameOverScreenController : MonoBehaviour
    {
        [SerializeField] private GameOverScreenView _gameOverScreenView;

        private GameOverScreenModel _model;

        public void Initialize(bool isVictory)
        {
            _model = new GameOverScreenModel(isVictory);
            _gameOverScreenView.StartGameClicked += HandleStartGameClicked;
            _gameOverScreenView.Render(_model.IsVictory);
        }

        private void OnDestroy()
        {
            if (_gameOverScreenView != null)
            {
                _gameOverScreenView.StartGameClicked -= HandleStartGameClicked;
            }
        }

        private void HandleStartGameClicked()
        {
            ServiceLocator.Instance.GetService<IGameService>().RestartGame();
            Destroy(gameObject);
        }
    }
}
