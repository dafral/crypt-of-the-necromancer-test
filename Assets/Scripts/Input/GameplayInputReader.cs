using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dafral.CustomInput
{
    public class GameplayInputReader : InputReader, InputMap.IGameplayActions
    {
        public override InputMode Mode => InputMode.Gameplay;
        public event Action OnMoveLeftInput = delegate { };
        public event Action OnMoveRightInput = delegate { };
        public event Action OnJumpInput = delegate { };
        public event Action OnWaitInput = delegate { };
        public event Action OnPauseInput = delegate { };

        private Dictionary<GameplayInputActions, Action<Action>> _addCallbacks;
        private Dictionary<GameplayInputActions, Action<Action>> _removeCallbacks;

        public GameplayInputReader(InputMap inputMap) : base(inputMap)
        {
            inputMap.Gameplay.SetCallbacks(this);
            InitializeCallbackMaps();
        }

        private void InitializeCallbackMaps()
        {
            _addCallbacks = new Dictionary<GameplayInputActions, Action<Action>>
            {
                { GameplayInputActions.Jump, callback => OnJumpInput += callback },
                { GameplayInputActions.MoveLeft, callback => OnMoveLeftInput += callback },
                { GameplayInputActions.MoveRight, callback => OnMoveRightInput += callback },
                { GameplayInputActions.Wait, callback => OnWaitInput += callback },
                { GameplayInputActions.Pause, callback => OnPauseInput += callback },
            };

            _removeCallbacks = new Dictionary<GameplayInputActions, Action<Action>>
            {
                { GameplayInputActions.Jump, callback => OnJumpInput -= callback },
                { GameplayInputActions.MoveLeft, callback => OnMoveLeftInput -= callback },
                { GameplayInputActions.MoveRight, callback => OnMoveRightInput -= callback },
                { GameplayInputActions.Wait, callback => OnWaitInput -= callback },
                { GameplayInputActions.Pause, callback => OnPauseInput -= callback },
            };
        }



        public void OnMoveLeft(InputAction.CallbackContext context)
        {
            OnPerformedEvent(OnMoveLeftInput, context);
        }

        public void OnMoveRight(InputAction.CallbackContext context)
        {
            OnPerformedEvent(OnMoveRightInput, context);
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            OnPerformedEvent(OnJumpInput, context);
        }

        public void OnWait(InputAction.CallbackContext context)
        {
            OnPerformedEvent(OnWaitInput, context);
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            OnPerformedEvent(OnPauseInput, context);
        }

        public void InvokeAction(GameplayInputActions inputAction)
        {
            switch (inputAction)
            {
                case GameplayInputActions.MoveLeft: OnMoveLeftInput?.Invoke(); break;
                case GameplayInputActions.MoveRight: OnMoveRightInput?.Invoke(); break;
                case GameplayInputActions.Jump: OnJumpInput?.Invoke(); break;
                case GameplayInputActions.Wait: OnWaitInput?.Invoke(); break;
                case GameplayInputActions.Pause: OnPauseInput?.Invoke(); break;
            }
        }

        public void AddCallback(GameplayInputActions inputAction, Action callback)
        {
            if (_addCallbacks.TryGetValue(inputAction, out var addAction))
            {
                addAction(callback);
            }
            else
            {
                Debug.LogWarning($"[GameplayInputReader] Callbacks cannot be added to action {inputAction}");
            }
        }

        public void RemoveCallback(GameplayInputActions inputAction, Action callback)
        {
            if (_removeCallbacks.TryGetValue(inputAction, out var removeAction))
            {
                removeAction(callback);
            }
            else
            {
                Debug.LogWarning($"[GameplayInputReader] Callbacks cannot be removed from action {inputAction}");
            }
        }
    }
}
