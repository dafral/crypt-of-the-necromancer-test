using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.Map
{
    /// <summary>
    /// Self-contained spike tile behaviour. On every beat it damages the entity
    /// standing on the tile directly above it, sourcing the damage from its own
    /// tile configuration so there is a single source of truth.
    /// </summary>
    public class SpikeTile : MonoBehaviour
    {
        private IEventService _eventService;
        private IGridService _gridService;
        private Vector2Int _gridPosition;
        private bool _subscribed;

        private void OnEnable()
        {
            if (!ServiceLocator.Instance.Contains<IEventService>() ||
                !ServiceLocator.Instance.Contains<IGridService>())
            {
                return;
            }

            _eventService = ServiceLocator.Instance.GetService<IEventService>();
            _gridService = ServiceLocator.Instance.GetService<IGridService>();
            _gridPosition = _gridService.GetGridPosition(transform.position);

            _eventService.Subscribe<OnBeatTriggered>(OnBeatTriggered);
            _subscribed = true;
        }

        private void OnDisable()
        {
            if (!_subscribed) return;

            _eventService.Unsubscribe<OnBeatTriggered>(OnBeatTriggered);
            _subscribed = false;
        }

        private void OnBeatTriggered(OnBeatTriggered e)
        {
            var grid = _gridService.Grid;
            if (grid == null) return;

            if (grid.GetTile(_gridPosition)?.TileType is not ITileHazard hazard) return;

            var aboveTile = grid.GetTile(_gridPosition + Vector2Int.up);
            if (aboveTile == null || !aboveTile.IsOccupied) return;

            hazard.ApplyHazard(aboveTile.OccupyingEntity);
        }
    }
}
