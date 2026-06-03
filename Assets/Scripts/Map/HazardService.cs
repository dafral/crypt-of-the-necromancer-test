using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.Map
{
    public class HazardService
    {
        private readonly IEventService _eventService;
        private readonly IGridService _gridService;

        public HazardService(IEventService eventService, IGridService gridService)
        {
            _eventService = eventService;
            _gridService = gridService;
            _eventService.Subscribe<OnBeatTriggered>(OnBeatTriggered);
        }

        private void OnBeatTriggered(OnBeatTriggered e)
        {
            var grid = _gridService.Grid;

            foreach (var kvp in grid.Tiles)
            {
                if (kvp.Value.TileType is not ITileHazard hazard) continue;

                var aboveTile = grid.GetTile(kvp.Key + Vector2Int.up);
                if (aboveTile == null || !aboveTile.IsOccupied) continue;

                hazard.ApplyHazard(aboveTile.OccupyingEntity);
            }
        }
    }
}
