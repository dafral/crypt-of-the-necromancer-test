using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.Map
{
    public class GridService : IGridService
    {
        private readonly IEventService _eventService;
        private GridData _grid;
        private GridCoordinateConverter _coordinateConverter;

        public GridData Grid => _grid;
        public GridCoordinateConverter CoordinateConverter => _coordinateConverter;

        public GridService(IEventService eventService)
        {
            _eventService = eventService;
        }

        public void LoadMap(MapConfiguration mapData)
        {
            _coordinateConverter = new GridCoordinateConverter(mapData.CellSize);
            _grid = new GridData(mapData.GridSize);

            var tiles = mapData.Tiles ?? System.Array.Empty<MapConfiguration.TileEntry>();
            foreach (var tileEntry in tiles)
            {
                _grid.SetTile(tileEntry.Position, tileEntry.TileType);
            }

            _eventService.RaiseEvent(new OnGridLoaded(_grid, _coordinateConverter));
        }

        public bool TryMoveEntity(IGridEntity entity, Vector2Int direction)
        {
            var targetPosition = entity.GridPosition + direction;
            bool success = _grid.TryMoveEntity(entity, targetPosition);

            if (success)
            {
                _eventService.RaiseEvent(new OnEntityGridPositionChanged(entity, targetPosition - direction, targetPosition));
            }

            return success;
        }

        public bool TryPlaceEntity(IGridEntity entity, Vector2Int position)
        {
            return _grid.TryPlaceEntity(entity, position);
        }

        public void RemoveEntity(IGridEntity entity)
        {
            _grid.RemoveEntity(entity);
        }

        public Vector3 GetWorldPosition(Vector2Int gridPosition)
        {
            return _coordinateConverter.GridToWorld(gridPosition);
        }

        public Vector2Int GetGridPosition(Vector3 worldPosition)
        {
            return _coordinateConverter.WorldToGrid(worldPosition);
        }
    }
}
