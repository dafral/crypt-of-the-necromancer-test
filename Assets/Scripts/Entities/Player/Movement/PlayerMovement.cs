using System;
using System.Collections.Generic;
using Dafral.CustomInput;
using Dafral.Events;
using Dafral.Game;
using Dafral.Game.Map;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Player
{
    public class PlayerMovement : MonoBehaviour, IPlayerMovement
    {
        private GameplayInputHandler _inputHandler;
        private IGridMovementController _movementController;
        private IPlayer _player;
        private PlayerMovementData _movementData;
        private IEventService _eventService;
        private IRhythmController _rhythmController;
        private int _lastConsumedBeat = -1;

        public void Initialize(
            IGridEntity gridEntity, 
            IPlayer player, 
            Transform playerTransform, 
            PlayerMovementData movementData)
        {
            _player = player;
            _movementData = movementData;
            _rhythmController = ServiceLocator.Instance.GetService<IGameService>().RhythmController;
            _eventService = ServiceLocator.Instance.GetService<IEventService>();

            _movementController = new GridMovementController(
                gridEntity, 
                playerTransform, 
                this, 
                _movementData.MoveDuration, 
                _movementData.FallStepDuration);

            _movementController.OnJumped += _player.OnJumped;
            _movementController.OnLanded += _player.OnLanded;

            _eventService.Subscribe<OnBeatTriggered>(OnBeatTriggered);
            InitializeInputHandler();
        }

        private void OnBeatTriggered(OnBeatTriggered e)
        {
            _movementController.TryApplyGravityStep();
        }

        private void InitializeInputHandler()
        {
            var inputActions = new Dictionary<GameplayInputActions, Action>
            {
                { GameplayInputActions.Jump, OnJump },
                { GameplayInputActions.MoveLeft, OnMoveLeft },
                { GameplayInputActions.Wait, OnWait },
                { GameplayInputActions.MoveRight, OnMoveRight },
            };
            
            _inputHandler = new GameplayInputHandler(inputActions);
            _inputHandler.Initialize();
        }

        private void OnMoveLeft()
        {
            if (TryRhythmMove(Vector2Int.left) != GridMoveResult.Blocked)
            {
                _player.OnDash(Vector2Int.left);
            }
        }

        private void OnMoveRight()
        {
            if (TryRhythmMove(Vector2Int.right) != GridMoveResult.Blocked)
            {
                _player.OnDash(Vector2Int.right);
            }
        }

        private void OnJump()
        {
            BeatScore score = EvaluateBeatTiming(out int beat);
            if (score == BeatScore.None) return;

            if (_lastConsumedBeat == beat) return;
            if (_movementController.TryJump(_movementData.JumpHeight))
            {
                _lastConsumedBeat = beat;
            }
        }

        private void OnWait()
        {
        }

        private GridMoveResult TryRhythmMove(Vector2Int direction)
        {
            BeatScore score = EvaluateBeatTiming(out int beat);

            if (score == BeatScore.None)
                return GridMoveResult.Blocked;

            if (_lastConsumedBeat == beat)
                return GridMoveResult.Blocked;

            GridMoveResult moveResult = _movementController.TryToMove(direction);
            if (moveResult != GridMoveResult.Blocked)
            {
                _lastConsumedBeat = beat;
            }

            return moveResult;
        }

        private BeatScore EvaluateBeatTiming(out int beat)
        {
            float elapsed = _rhythmController.ElapsedFromLastBeat;
            float beatInterval = _rhythmController.BeatInterval;
            float timeToNextBeat = beatInterval - elapsed;

            bool inLateWindow = elapsed <= _movementData.LateWindowDuration;
            bool inEarlyWindow = timeToNextBeat <= _movementData.EarlyWindowDuration;

            beat = _rhythmController.BeatCount;

            if (!inLateWindow && !inEarlyWindow)
                return BeatScore.None;

            float distanceToBeat = Mathf.Min(elapsed, timeToNextBeat);
            if (inEarlyWindow && timeToNextBeat <= elapsed)
            {
                beat++;
            }

            if (distanceToBeat <= _movementData.PerfectWindowDuration)
                return BeatScore.Perfect;

            if (inEarlyWindow)
                return BeatScore.TooSoon;

            return BeatScore.TooLate;
        }

        public void Dispose()
        {
            _inputHandler?.Dispose();
            _eventService?.Unsubscribe<OnBeatTriggered>(OnBeatTriggered);
            _movementController?.Stop();
        }
    }
}
