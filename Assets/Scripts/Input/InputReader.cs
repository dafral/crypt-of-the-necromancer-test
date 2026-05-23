using System;
using UnityEngine.InputSystem;

namespace Dafral.CustomInput
{
    public abstract class InputReader
    {
        protected InputMap _inputMap;
        public abstract InputMode Mode { get; }

        protected InputReader(InputMap inputMap)
        {
            _inputMap = inputMap;
        }

        protected void OnPerformedEvent(Action actionEvent, InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                actionEvent?.Invoke();
            }

        }
    }
}