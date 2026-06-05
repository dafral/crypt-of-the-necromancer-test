using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Dafral.Game.UI
{
    public class StartGameScreenView : MonoBehaviour
    {
        [SerializeField] private Button _startButton;

        public void Open(UnityAction startButtonClicked)
        {
            _startButton.onClick.AddListener(startButtonClicked);
        }

        public void Close()
        {
            _startButton.onClick.RemoveAllListeners();
        }
    }
}
