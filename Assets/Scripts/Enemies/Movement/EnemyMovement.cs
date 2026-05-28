using Dafral.Events;
using Dafral.Game.Map;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        private IEnemyMovementStrategy _movementStrategy;
        private int _beatsToMove;
        private int _beatsCount;

        public void Initialize(IGridEntity gridEntity, EnemyData data)
        {
            _beatsToMove = data.BeatsToMove;
            CreateMovementStrategy(data.MovementStrategy, gridEntity);
            SubscribeToEvents();
        }

        private void CreateMovementStrategy(EnemyMovementType movementType, IGridEntity gridEntity)
        {
            switch (movementType)
            {
                case EnemyMovementType.Idle:
                    _movementStrategy = new IdleMovementStrategy();
                    break;
                case EnemyMovementType.Random:
                    _movementStrategy = new RandomMovementStrategy(gridEntity);
                    break;
            }
        }

        private void SubscribeToEvents()
        {
            ServiceLocator.Instance.GetService<IEventService>().Subscribe<OnBeatTriggered>(OnBeatTriggered);
        }

        public void Dispose()
        {
            ServiceLocator.Instance.GetService<IEventService>().Unsubscribe<OnBeatTriggered>(OnBeatTriggered);
        }

        private void OnBeatTriggered(OnBeatTriggered e)
        {
            _beatsCount++;
            if (_beatsCount >= _beatsToMove)
            {
                _beatsCount = 0;
                _movementStrategy.TryMove();
            }
        }
    }
}
