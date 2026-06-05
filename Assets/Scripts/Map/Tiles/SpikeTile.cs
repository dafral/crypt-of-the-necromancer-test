using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.Map
{
    public class SpikeTile : MonoBehaviour
    {
        private IEventService _eventService;
        private IGridService _gridService;
        private Vector2Int _gridPosition;
        private bool _subscribed;

        private void OnEnable()
        {
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
