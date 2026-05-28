using Dafral.Game.Map;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Enemies
{
    public class Enemy : MonoBehaviour, IEnemy, IGridEntity
    {
        [SerializeField] private EnemyHealth _enemyHealth;
        [SerializeField] private EnemyMovement _enemyMovement;
        [SerializeField] private EnemyInteract _enemyInteract;
        private Vector2Int _gridPosition;

        public Vector2Int GridPosition => _gridPosition;
        public GridEntityType EntityType => GridEntityType.Enemy;

        public void Initialize(EnemyData enemyData)
        {
            var serviceLocator = ServiceLocator.Instance;
            _gridPosition = serviceLocator.GetService<IGridService>().GetGridPosition(transform.position);
            serviceLocator.GetService<IGridService>().TryPlaceEntity(this, _gridPosition);
            _enemyHealth.Initialize(enemyData, OnDeath);
            _enemyMovement.Initialize(this, enemyData);
        }

        private void OnDeath()
        {
            Dispose();
            ServiceLocator.Instance.GetService<IGridService>().RemoveEntity(this);
            Destroy(gameObject);
        }

        public void SetGridPosition(Vector2Int position)
        {
            _gridPosition = position;
        }

        public void Interact(IGridEntity otherEntity)
        {
        }

        public bool TryMove(Vector2Int direction)
        {
            if (direction == Vector2Int.zero)
                return false;

            var gridService = ServiceLocator.Instance.GetService<IGridService>();
            bool success = gridService.TryMoveEntity(this, direction);
            if (success)
            {
                transform.position = gridService.GetWorldPosition(_gridPosition);
            }

            return success;
        }

        public void TakeDamage(int damage)
        {
            _enemyHealth.TakeDamage(damage);
        }

        public void Dispose()
        {
            _enemyHealth.Dispose();
            _enemyMovement.Dispose();
        }
    }
}
