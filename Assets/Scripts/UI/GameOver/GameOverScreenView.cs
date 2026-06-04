using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dafral.Game.UI
{
    public class GameOverScreenView : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Button _restartButton;

        public void Open(UnityAction restartButtonClicked)
        {
            _canvasGroup.alpha = 1;
            _canvasGroup.blocksRaycasts = true;
            _canvasGroup.interactable = true;
            _restartButton.onClick.AddListener(restartButtonClicked);
        }

        public void Close()
        {
            _canvasGroup.alpha = 0;
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.interactable = false;
            _restartButton.onClick.RemoveAllListeners();
        }
    }
}
