using Dafral.Events;
using Dafral.Game;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.Map
{
    public abstract class GridEntity : MonoBehaviour, IGridEntity
    {
        [SerializeField] private EntityHealth _health;

        protected IGridService _gridService;
        protected IEventService _eventService;
        private Vector2Int _gridPosition;

        protected IEntityHealth Health => _health;
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
            _eventService = ServiceLocator.Instance.GetService<IEventService>();
            _gridPosition = _gridService.GetGridPosition(transform.position);
            _gridService.TryPlaceEntity(this, _gridPosition);

            _eventService.Subscribe<OnMapCleared>(OnMapCleared);
        }

        private void OnMapCleared(OnMapCleared e)
        {
            Despawn();
        }

        protected virtual void Despawn()
        {
            _gridService?.RemoveEntity(this);
            Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            _eventService?.Unsubscribe<OnMapCleared>(OnMapCleared);
        }
    }
}
