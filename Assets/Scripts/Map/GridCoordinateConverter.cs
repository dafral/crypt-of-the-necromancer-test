using UnityEngine;

namespace Dafral.Game.Map
{
    public class GridCoordinateConverter
    {
        private readonly float _cellSize;
        private readonly Vector3 _originOffset;

        public float CellSize => _cellSize;

        public GridCoordinateConverter(float cellSize, Vector3 originOffset = default)
        {
            _cellSize = cellSize;
            _originOffset = originOffset;
        }

        public Vector3 GridToWorld(Vector2Int gridPosition)
        {
            return new Vector3(
                gridPosition.x * _cellSize + _cellSize * 0.5f,
                gridPosition.y * _cellSize + _cellSize * 0.5f,
                0f
            ) + _originOffset;
        }

        public Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            var adjusted = worldPosition - _originOffset;
            return new Vector2Int(
                Mathf.FloorToInt(adjusted.x / _cellSize),
                Mathf.FloorToInt(adjusted.y / _cellSize)
            );
        }

        public Vector3 SnapToGrid(Vector3 worldPosition)
        {
            var gridPos = WorldToGrid(worldPosition);
            return GridToWorld(gridPos);
        }
    }
}
