using System;
using System.Collections.Generic;
using Dafral.CustomInput;
using UnityEngine;

namespace Dafral.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        private GameplayInputHandler _inputHandler;
        private Transform _playerTransform;

        public void Initialize(Transform playerTransform)
        {
            _playerTransform = playerTransform;
            InitializeInputHandler();
        }

        private void InitializeInputHandler()
        {
            var inputActions = new Dictionary<GameplayInputActions, Action>();
            _inputHandler = new GameplayInputHandler(inputActions);
            _inputHandler.Initialize();
        }

        private void OnEnable()
        {
            _inputHandler?.Initialize();
        }

        private void OnDisable()
        {
            _inputHandler?.Dispose();
        }
    }
}
