using Dafral.Game;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.Map
{
    public abstract class GridEntity : MonoBehaviour, IGridEntity
    {
        [SerializeField] protected Health _health;

        protected IGridService _gridService;
        private Vector2Int _gridPosition;

        public Vector2Int GridPosition => _gridPosition;
        public abstract GridEntityType EntityType { get; }

        public void SetGridPosition(Vector2Int position)
        {
            _gridPosition = position;
        }

        public void TakeDamage(int damage)
        {
            _health.TakeDamage(damage);
        }

        public abstract void Interact(IGridEntity otherEntity);

        protected void RegisterOnGrid()
        {
            _gridService = ServiceLocator.Instance.GetService<IGridService>();
            _gridPosition = _gridService.GetGridPosition(transform.position);
            _gridService.TryPlaceEntity(this, _gridPosition);
        }
    }
}
