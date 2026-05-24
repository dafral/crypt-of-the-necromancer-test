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
        private IGridService _gridService;

        public Vector2Int GridPosition => _gridPosition;
        public GridEntityType EntityType => GridEntityType.Enemy;
        public bool IsMoving => false;

        public void Interact(IGridEntity otherEntity)
        {
        }

        public void SetGridPosition(Vector2Int position)
        {
            _gridPosition = position;
        }

        public bool TryMove(Vector2Int direction)
        {
            return false;
        }

        public void TakeDamage(int damage)
        {
            _enemyHealth.TakeDamage(damage);
        }

        private void Start()
        {
            _gridService = ServiceLocator.Instance.GetService<IGridService>();
            _gridPosition = _gridService.GetGridPosition(transform.position);
            _gridService.TryPlaceEntity(this, _gridPosition);
            _enemyHealth.Initialize(OnDeath);
        }

        private void OnDeath()
        {
            _gridService.RemoveEntity(this);
            Destroy(gameObject);
        }
    }
}
