using UnityEngine;

namespace Dafral.Grid
{
    public interface IGridService
    {
        GridData Grid { get; }
        GridCoordinateConverter CoordinateConverter { get; }
        void LoadLevel(LevelDataConfiguration levelData);
        bool TryMoveEntity(IGridEntity entity, Vector2Int direction);
        bool TryPlaceEntity(IGridEntity entity, Vector2Int position);
        void RemoveEntity(IGridEntity entity);
        Vector3 GetWorldPosition(Vector2Int gridPosition);
        Vector2Int GetGridPosition(Vector3 worldPosition);
    }
}
