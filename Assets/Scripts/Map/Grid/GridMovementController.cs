using System;
using System.Collections;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.Map
{
    public class GridMovementController : IGridMovementController
    {
        public event Action OnJumped;
        public event Action OnLanded;
        private readonly IGridEntity _gridEntity;
        private readonly IGridService _gridService;
        private readonly Transform _transform;
        private readonly MonoBehaviour _coroutineHost;
        private readonly float _moveDuration;
        private readonly float _fallStepDuration;

        private bool _isMoving;
        private Coroutine _moveCoroutine;

        public GridMovementController(
            IGridEntity gridEntity,
            Transform transform,
            MonoBehaviour coroutineHost,
            float moveDuration,
            float fallStepDuration)
        {
            _gridEntity = gridEntity;
            _gridService = ServiceLocator.Instance.GetService<IGridService>();
            _transform = transform;
            _coroutineHost = coroutineHost;
            _moveDuration = moveDuration;
            _fallStepDuration = fallStepDuration;
        }

        public bool TryToMove(Vector2Int direction)
        {
            if (_isMoving) return false;

            bool success = _gridService.TryMoveEntity(_gridEntity, direction);
            if (success)
            {
                var targetWorldPos = _gridService.GetWorldPosition(_gridEntity.GridPosition);
                _moveCoroutine = _coroutineHost.StartCoroutine(AnimateAndFinish(targetWorldPos, _moveDuration));
            }

            return success;
        }

        public bool TryJump(int height)
        {
            if (_isMoving || height <= 0 || !IsGrounded()) return false;

            _moveCoroutine = _coroutineHost.StartCoroutine(JumpCoroutine(height));
            OnJumped?.Invoke();
            return true;
        }

        public bool TryApplyGravityStep()
        {
            if (_isMoving || IsGrounded()) return false;

            bool fell = _gridService.TryMoveEntity(_gridEntity, Vector2Int.down);
            if (fell)
            {
                var fallTarget = _gridService.GetWorldPosition(_gridEntity.GridPosition);
                _moveCoroutine = _coroutineHost.StartCoroutine(FallAndCheckLanded(fallTarget, _fallStepDuration));
            }

            return fell;
        }

        public bool IsGrounded()
        {
            var belowPosition = _gridEntity.GridPosition + Vector2Int.down;
            var grid = _gridService.Grid;

            if (!grid.IsWithinBounds(belowPosition)) return true;

            var tileBelow = grid.GetTile(belowPosition);
            if (tileBelow == null) return true;

            return tileBelow.GetTileState() != TileState.Walkable;
        }

        public void Stop()
        {
            if (_moveCoroutine != null)
            {
                _coroutineHost.StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
                _isMoving = false;
            }
        }

        private IEnumerator JumpCoroutine(int height)
        {
            _isMoving = true;

            for (int i = 0; i < height; i++)
            {
                bool moved = _gridService.TryMoveEntity(_gridEntity, Vector2Int.up);
                if (!moved) break;

                var target = _gridService.GetWorldPosition(_gridEntity.GridPosition);
                yield return LerpTo(target, _moveDuration);
            }

            _isMoving = false;
            _moveCoroutine = null;

            if (IsGrounded())
                OnLanded?.Invoke();
        }

        private IEnumerator FallAndCheckLanded(Vector3 targetPosition, float duration)
        {
            _isMoving = true;
            yield return LerpTo(targetPosition, duration);
            _isMoving = false;
            _moveCoroutine = null;

            if (IsGrounded())
                OnLanded?.Invoke();
        }

        private IEnumerator AnimateAndFinish(Vector3 targetPosition, float duration)
        {
            _isMoving = true;
            yield return LerpTo(targetPosition, duration);
            _isMoving = false;
            _moveCoroutine = null;
        }

        private IEnumerator LerpTo(Vector3 targetPosition, float duration)
        {
            var startPosition = _transform.position;
            var elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var t = Mathf.Clamp01(elapsed / duration);
                _transform.position = Vector3.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            _transform.position = targetPosition;
        }
    }
}
