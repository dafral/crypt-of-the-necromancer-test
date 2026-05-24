using System;
using System.Collections;
using System.Collections.Generic;
using Dafral.CustomInput;
using Dafral.Events;
using Dafral.Game;
using Dafral.Game.Map;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveDuration = 0.1f;

        [Header("Rhythm Timing Windows (seconds)")]
        [SerializeField] private float _earlyWindowDuration = 0.15f;
        [SerializeField] private float _lateWindowDuration = 0.15f;
        [SerializeField] private float _perfectWindowDuration = 0.05f;

        private GameplayInputHandler _inputHandler;
        private IGridEntity _gridEntity;
        private IGridService _gridService;
        private IEventService _eventService;
        private RhythmController _rhythmController;
        private Transform _playerTransform;
        private bool _isMoving;
        private bool _movedThisBeat;
        private Coroutine _moveCoroutine;

        public bool IsMoving => _isMoving;
        public event Action<BeatScore> OnMovementScored;

        public void Initialize(IGridEntity gridEntity, Transform playerTransform)
        {
            _gridEntity = gridEntity;
            _playerTransform = playerTransform;

            var serviceLocator = ServiceLocator.Instance;
            _gridService = serviceLocator.GetService<IGridService>();
            _eventService = serviceLocator.GetService<IEventService>();
            _rhythmController = FindObjectOfType<RhythmController>();

            _eventService.Subscribe<OnBeatTriggered>(OnBeatTriggered);
            InitializeInputHandler();
        }

        private void OnBeatTriggered(OnBeatTriggered e)
        {
            _movedThisBeat = false;
        }

        private void InitializeInputHandler()
        {
            var inputActions = new Dictionary<GameplayInputActions, Action>
            {
                { GameplayInputActions.MoveUp, OnMoveUp },
                { GameplayInputActions.MoveLeft, OnMoveLeft },
                { GameplayInputActions.MoveDown, OnMoveDown },
                { GameplayInputActions.MoveRight, OnMoveRight },
            };
            
            _inputHandler = new GameplayInputHandler(inputActions);
            _inputHandler.Initialize();
        }

        private void OnMoveUp()
        {
            TryRhythmMove(Vector2Int.up);
        }

        private void OnMoveLeft()
        {
            TryRhythmMove(Vector2Int.left);
        }

        private void OnMoveDown()
        {
            TryRhythmMove(Vector2Int.down);
        }

        private void OnMoveRight()
        {
            TryRhythmMove(Vector2Int.right);
        }

        private void TryRhythmMove(Vector2Int direction)
        {
            BeatScore score = EvaluateBeatTiming();

            if (score == BeatScore.None)
                return;

            if (TryToMove(direction))
            {
                _movedThisBeat = true;
                OnMovementScored?.Invoke(score);
            }
        }

        private BeatScore EvaluateBeatTiming()
        {
            float elapsed = _rhythmController.ElapsedFromLastBeat;
            float beatInterval = _rhythmController.BeatInterval;
            float timeToNextBeat = beatInterval - elapsed;

            bool inLateWindow = elapsed <= _lateWindowDuration;
            bool inEarlyWindow = timeToNextBeat <= _earlyWindowDuration;

            if (!inLateWindow && !inEarlyWindow)
                return BeatScore.None;

            float distanceToBeat = Mathf.Min(elapsed, timeToNextBeat);

            if (distanceToBeat <= _perfectWindowDuration)
                return BeatScore.Perfect;

            if (inEarlyWindow)
                return BeatScore.TooSoon;

            return BeatScore.TooLate;
        }

        public bool TryToMove(Vector2Int direction)
        {
            if (_isMoving) return false;

            bool success = _gridService.TryMoveEntity(_gridEntity, direction);
            if (success)
            {
                var targetWorldPos = _gridService.GetWorldPosition(_gridEntity.GridPosition);
                _moveCoroutine = StartCoroutine(AnimateMove(targetWorldPos));
            }

            return success;
        }

        private IEnumerator AnimateMove(Vector3 targetPosition)
        {
            _isMoving = true;

            var startPosition = _playerTransform.position;
            var elapsed = 0f;

            while (elapsed < _moveDuration)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / _moveDuration);
                _playerTransform.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            _playerTransform.position = targetPosition;
            _isMoving = false;
            _moveCoroutine = null;
        }

        private Vector2Int SnapToCardinalDirection(Vector2 input)
        {
            if (input.sqrMagnitude < 0.1f) return Vector2Int.zero;

            if (Mathf.Abs(input.x) >= Mathf.Abs(input.y))
            {
                return input.x > 0 ? Vector2Int.right : Vector2Int.left;
            }

            return input.y > 0 ? Vector2Int.up : Vector2Int.down;
        }

        private void OnEnable()
        {
            _inputHandler?.Initialize();
        }

        private void OnDisable()
        {
            _inputHandler?.Dispose();
            _eventService?.Unsubscribe<OnBeatTriggered>(OnBeatTriggered);

            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
                _isMoving = false;
            }
        }
    }
}
