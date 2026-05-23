using System;
using System.Collections.Generic;
using Dafral.Events;
using Dafral.Services;

namespace Dafral.CustomInput
{
    public class GameplayInputHandler
    {
        private GameplayInputReader _inputReader;
        private Dictionary<GameplayInputActions, Action> _inputActions;

        public GameplayInputReader Reader => _inputReader;

        public GameplayInputHandler(Dictionary<GameplayInputActions, Action> inputActions)
        {
            _inputActions = inputActions ?? new Dictionary<GameplayInputActions, Action>();
        }

        public void Initialize()
        {
            var eventService = ServiceLocator.Instance.GetService<IEventService>();
            eventService.Subscribe<OnInputModeChanged>(OnInputModeChanged);
            TryToRegisterToCurrentInput();
        }

        private void TryToRegisterToCurrentInput()
        {
            var inputService = ServiceLocator.Instance.GetService<IInputService>();
            if(inputService.CurrentInput is GameplayInputReader)
            {
                RegisterToInputEvents(inputService.CurrentInput as GameplayInputReader);
            }
        }

        public void Dispose()
        {
            var eventService = ServiceLocator.Instance.GetService<IEventService>();
            eventService.Unsubscribe<OnInputModeChanged>(OnInputModeChanged);
            UnregisterFromInputEvents();
        }

        private void OnInputModeChanged(OnInputModeChanged e)
        {
            if(e.NewInput is GameplayInputReader)
            {
                RegisterToInputEvents(e.NewInput as GameplayInputReader);
            }   
            else
            {
                UnregisterFromInputEvents();
            }
        }

        private void RegisterToInputEvents(GameplayInputReader inputReader)
        {
            _inputReader = inputReader;

            foreach(var inputAction in _inputActions)
            {
                _inputReader.AddCallback(inputAction.Key, inputAction.Value);
            }
        }

        private void UnregisterFromInputEvents()
        {
            if(_inputReader == null) return;
            foreach(var inputAction in _inputActions)
            {
                _inputReader.RemoveCallback(inputAction.Key, inputAction.Value);
            }
            _inputReader = null;
        }
    }
}
