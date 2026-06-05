using System;
using UnityEngine;
using UnityEngine.UI;

namespace Dafral.Game.UI
{
    public class ScreenControlsView : MonoBehaviour
    {
        [Header("Left Side")]
        [SerializeField] private Button _leftSideMoveLeftButton;
        [SerializeField] private Button _leftSideMoveRightButton;
        [SerializeField] private Button _leftSideJumpButton;

        [Header("Right Side")]
        [SerializeField] private Button _rightSideMoveLeftButton;
        [SerializeField] private Button _rightSideMoveRightButton;
        [SerializeField] private Button _rightSideJumpButton;

        public void Initialize(Action onMoveLeft, Action onMoveRight, Action onJump)
        {
            _leftSideMoveLeftButton.onClick.AddListener(() => onMoveLeft?.Invoke());
            _leftSideMoveRightButton.onClick.AddListener(() => onMoveRight?.Invoke());
            _leftSideJumpButton.onClick.AddListener(() => onJump?.Invoke());

            _rightSideMoveLeftButton.onClick.AddListener(() => onMoveLeft?.Invoke());
            _rightSideMoveRightButton.onClick.AddListener(() => onMoveRight?.Invoke());
            _rightSideJumpButton.onClick.AddListener(() => onJump?.Invoke());
        }

        public void Dispose()
        {
            _leftSideMoveLeftButton.onClick.RemoveAllListeners();
            _leftSideMoveRightButton.onClick.RemoveAllListeners();
            _leftSideJumpButton.onClick.RemoveAllListeners();

            _rightSideMoveLeftButton.onClick.RemoveAllListeners();
            _rightSideMoveRightButton.onClick.RemoveAllListeners();
            _rightSideJumpButton.onClick.RemoveAllListeners();
        }
    }
}
