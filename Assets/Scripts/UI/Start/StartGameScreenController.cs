using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.UI
{
    public class StartGameScreenController : MonoBehaviour
    {
        [SerializeField] private StartGameScreenView _startGameScreenView;

        public void Initialize()
        {
            _startGameScreenView.Open(HandleStartGameClicked);
        }

        private void HandleStartGameClicked()
        {
            ServiceLocator.Instance.GetService<IGameService>().StartGame();
            _startGameScreenView.Close();
            Destroy(gameObject);
        }
    }
}
