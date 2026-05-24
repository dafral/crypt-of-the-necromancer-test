using System.Collections;
using Dafral.CustomInput;
using Dafral.Game.Map;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _moveDuration = 0.1f;

        private GameplayInputHandler _inputHandler;
        private IGridEntity _gridEntity;
        private IGridService _gridService;
        private Transform _playerTransform;
        private bool _isMoving;
        private Coroutine _moveCoroutine;

        public bool IsMoving => _isMoving;

        public void Initialize(IGridEntity gridEntity, Transform playerTransform)
        {
            _gridEntity = gridEntity;
            _playerTransform = playerTransform;
            _gridService = ServiceLocator.Instance.GetService<IGridService>();
            InitializeInputHandler();
        }

        private void InitializeInputHandler()
        {
            _inputHandler = new GameplayInputHandler(new System.Collections.Generic.Dictionary<GameplayInputActions, System.Action>());
            _inputHandler.Initialize();
        }

        private void Update()
        {
            if (_isMoving) return;
            if (_inputHandler?.Reader == null) return;

            var input = _inputHandler.Reader.MovementInput;
            var direction = SnapToCardinalDirection(input);

            if (direction != Vector2Int.zero)
            {
                TryToMove(direction);
            }
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

            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
                _isMoving = false;
            }
        }
    }
}
