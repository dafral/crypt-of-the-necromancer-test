using Dafral.Events;
using Dafral.Game.Map;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Enemies
{
    public class EnemyMovement : MonoBehaviour, IEnemyMovement
    {
        [SerializeField] private float _moveDuration = 0.1f;
        [SerializeField] private float _fallStepDuration = 0.05f;

        private IEnemyMovementStrategy _movementStrategy;
        private GridMovementController _movementController;
        private int _beatsToMove;
        private int _beatsCount;

        public void Initialize(IGridEntity gridEntity, EnemyData data)
        {
            _beatsToMove = data.BeatsToMove;

            _movementController = new GridMovementController(
                gridEntity, transform, this, _moveDuration, _fallStepDuration);

            CreateMovementStrategy(data.MovementStrategy);
            SubscribeToEvents();
        }

        private void CreateMovementStrategy(EnemyMovementType movementType)
        {
            switch (movementType)
            {
                case EnemyMovementType.Idle:
                    _movementStrategy = new IdleMovementStrategy();
                    break;
                case EnemyMovementType.Random:
                    _movementStrategy = new RandomMovementStrategy();
                    break;
            }
        }

        private void SubscribeToEvents()
        {
            ServiceLocator.Instance.GetService<IEventService>().Subscribe<OnBeatTriggered>(OnBeatTriggered);
        }

        public void Dispose()
        {
            _movementController?.Stop();
            ServiceLocator.Instance.GetService<IEventService>().Unsubscribe<OnBeatTriggered>(OnBeatTriggered);
        }

        private void OnBeatTriggered(OnBeatTriggered e)
        {
            if (!_movementController.IsGrounded())
            {
                _movementController.TryApplyGravityStep();
                return;
            }

            _beatsCount++;
            if (_beatsCount >= _beatsToMove)
            {
                _beatsCount = 0;
                var direction = _movementStrategy.GetNextDirection();
                if (direction.HasValue)
                {
                    _movementController.TryToMove(direction.Value);
                }
            }
        }
    }
}
