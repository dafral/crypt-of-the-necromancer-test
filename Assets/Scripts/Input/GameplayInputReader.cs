using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Dafral.CustomInput
{
    public class GameplayInputReader : InputReader, InputMap.IGameplayActions
    {
        public override InputMode Mode => InputMode.Gameplay;
        public event Action OnPauseInput = delegate { };
        public Vector2 MovementInput => _inputMap.Gameplay.Movement.ReadValue<Vector2>();

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
                { GameplayInputActions.Pause, callback => OnPauseInput += callback },
            };

            _removeCallbacks = new Dictionary<GameplayInputActions, Action<Action>>
            {
                { GameplayInputActions.Pause, callback => OnPauseInput -= callback },
            };
        }

        

        public void OnMovement(InputAction.CallbackContext context)
        {
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            OnPerformedEvent(OnPauseInput, context);
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
