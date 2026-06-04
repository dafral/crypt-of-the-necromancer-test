using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.UI
{
    public class GameOverScreenController : MonoBehaviour
    {
        [SerializeField] private GameOverScreenView _victoryGameOverScreenView;
        [SerializeField] private GameOverScreenView _deathGameOverScreenView;

        private GameOverScreenModel _model;

        public void Initialize(bool isVictory)
        {
            _model = new GameOverScreenModel(isVictory);
            if (_model.IsVictory)
            {
                _victoryGameOverScreenView.Open(HandleStartGameClicked);
            }
            else
            {
                _deathGameOverScreenView.Open(HandleStartGameClicked);
            }
        }

        private void HandleStartGameClicked()
        {
            ServiceLocator.Instance.GetService<IGameService>().RestartGame();
            Destroy(gameObject);
        }
    }
}
