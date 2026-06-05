using System;
using System.Collections.Generic;
using Dafral.CustomInput;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.UI
{
    public class ScreenControlsController : MonoBehaviour
    {
        [SerializeField] private ScreenControlsView _view;

        private GameplayInputHandler _inputHandler;

        public void Initialize()
        {
            _inputHandler = new GameplayInputHandler(new Dictionary<GameplayInputActions, Action>());
            _inputHandler.Initialize();

            _view.Initialize(
                () => _inputHandler.Reader?.InvokeAction(GameplayInputActions.MoveLeft),
                () => _inputHandler.Reader?.InvokeAction(GameplayInputActions.MoveRight),
                () => _inputHandler.Reader?.InvokeAction(GameplayInputActions.Jump)
            );
        }

        private void OnDestroy()
        {
            _view.Dispose();
            _inputHandler?.Dispose();
        }
    }
}
