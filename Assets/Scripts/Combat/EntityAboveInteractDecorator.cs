using Dafral.Events;
using Dafral.Game.Map;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.Combat
{
    public class EntityAboveInteractDecorator : IEntityInteract
    {
        private readonly IEntityInteract _decorated;
        private readonly IGridEntity _owner;
        private IEventService _eventService;
        private IGridService _gridService;

        public EntityAboveInteractDecorator(IEntityInteract decorated, IGridEntity owner)
        {
            _decorated = decorated;
            _owner = owner;
        }

        public void Initialize(GridEntityType targetType, ICombat combat)
        {
            _decorated.Initialize(targetType, combat);
            _eventService = ServiceLocator.Instance.GetService<IEventService>();
            _gridService = ServiceLocator.Instance.GetService<IGridService>();
            _eventService.Subscribe<OnBeatTriggered>(OnBeatTriggered);
        }

        public void Interact(IGridEntity otherEntity)
        {
            _decorated.Interact(otherEntity);
        }

        public void Dispose()
        {
            _eventService.Unsubscribe<OnBeatTriggered>(OnBeatTriggered);
            _decorated.Dispose();
        }

        private void OnBeatTriggered(OnBeatTriggered e)
        {
            Vector2Int targetPosition = _owner.GridPosition + Vector2Int.up;
            GridData grid = _gridService.Grid;
            bool inBounds = grid.IsWithinBounds(targetPosition);
            TileData targetTile = inBounds ? grid.GetTile(targetPosition) : null;
            IGridEntity occupyingEntity = targetTile?.OccupyingEntity;

            if (!inBounds) return;
            if (occupyingEntity == null) return;

            _decorated.Interact(occupyingEntity);
        }
    }
}
