using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dafral.Game.UI
{
    public class GameOverScreenView : MonoBehaviour
    {
        [SerializeField] private GameObject _victoryGroup;
        [SerializeField] private GameObject _defeatGroup;
        [SerializeField] private Button _startGameButton;

        public event Action StartGameClicked;

        private void Awake()
        {
            _startGameButton.onClick.AddListener(HandleStartGameClicked);
        }

        private void OnDestroy()
        {
            _startGameButton.onClick.RemoveListener(HandleStartGameClicked);
        }

        public void Render(bool isVictory)
        {
            _victoryGroup.SetActive(isVictory);
            _defeatGroup.SetActive(!isVictory);
        }

        private void HandleStartGameClicked()
        {
            StartGameClicked?.Invoke();
        }
    }
}
