using UnityEngine;

namespace Dafral.Grid
{
    public class TileData
    {
        public Vector2Int Position { get; }
        public TileConfiguration TileType { get; private set; }
        public IGridEntity OccupyingEntity { get; private set; }

        public bool IsWalkable => TileType != null && TileType.IsWalkable;
        public bool IsOccupied => OccupyingEntity != null;
        public bool CanBeEntered => IsWalkable && !IsOccupied;

        public TileData(Vector2Int position, TileConfiguration tileType)
        {
            Position = position;
            TileType = tileType;
        }

        public void SetTileType(TileConfiguration tileType)
        {
            TileType = tileType;
        }

        public bool TrySetOccupant(IGridEntity entity)
        {
            if (IsOccupied) return false;
            OccupyingEntity = entity;
            return true;
        }

        public void ClearOccupant()
        {
            OccupyingEntity = null;
        }
    }
}
