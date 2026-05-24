using UnityEngine;

namespace Dafral.Game.Map
{
    public interface IGridService
    {
        GridData Grid { get; }
        GridCoordinateConverter CoordinateConverter { get; }
        void LoadMap(MapConfiguration mapData);
        bool TryMoveEntity(IGridEntity entity, Vector2Int direction);
        bool TryPlaceEntity(IGridEntity entity, Vector2Int position);
        void RemoveEntity(IGridEntity entity);
        Vector3 GetWorldPosition(Vector2Int gridPosition);
        Vector2Int GetGridPosition(Vector3 worldPosition);
    }
}
