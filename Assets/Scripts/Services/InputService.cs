using Dafral.CustomInput;
using Dafral.Events;
using UnityEngine;

namespace Dafral.Services
{
    public class InputService : IInputService
    {
        private readonly IEventService _eventService;
        private readonly InputMap _inputMap;
        private InputReader _currentInput;

        public InputReader CurrentInput => _currentInput;

        public InputService(IEventService eventService)
        {
            _eventService = eventService;
            _inputMap = new InputMap();
            ChangeInputMode(InputMode.Gameplay);
        }

        public InputReader ChangeInputMode(InputMode newInput)
        {
            if (_currentInput != null && _currentInput.Mode == newInput) return _currentInput;

            DisableAllInput();

            switch(newInput)
            {
                case InputMode.Gameplay:
                    _inputMap.Gameplay.Enable();
                    _currentInput = new GameplayInputReader(_inputMap);
                    break;
                case InputMode.Menu:
                    _inputMap.Menu.Enable();
                    //_currentInput = new MenuInputReader(_inputMap);
                    break;
            }

            _eventService.RaiseEvent(new OnInputModeChanged(_currentInput));
            return _currentInput;
        }

        private void DisableAllInput()
        {
            _inputMap.Gameplay.Disable();
            _inputMap.Menu.Disable();
            _currentInput = null;
        }
    }
}