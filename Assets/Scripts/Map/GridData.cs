using System.Collections.Generic;
using UnityEngine;

namespace Dafral.Game.Map
{
    public class GridData
    {
        private readonly Dictionary<Vector2Int, TileData> _tiles;
        private readonly Vector2Int _size;

        public Vector2Int Size => _size;
        public IReadOnlyDictionary<Vector2Int, TileData> Tiles => _tiles;

        public GridData(Vector2Int size)
        {
            _size = size;
            _tiles = new Dictionary<Vector2Int, TileData>(size.x * size.y);
        }

        public void SetTile(Vector2Int position, TileConfiguration tileType)
        {
            if (!IsWithinBounds(position)) return;

            if (_tiles.TryGetValue(position, out var existing))
            {
                existing.SetTileType(tileType);
            }
            else
            {
                _tiles[position] = new TileData(position, tileType);
            }
        }

        public TileData GetTile(Vector2Int position)
        {
            _tiles.TryGetValue(position, out var tile);
            return tile;
        }

        public bool IsWithinBounds(Vector2Int position)
        {
            return position.x >= 0 && position.x < _size.x
                && position.y >= 0 && position.y < _size.y;
        }

        public bool CanMoveTo(Vector2Int position)
        {
            if (!IsWithinBounds(position)) return false;
            var tile = GetTile(position);
            return tile != null && tile.CanBeEntered;
        }

        public bool TryMoveEntity(IGridEntity entity, Vector2Int targetPosition)
        {
            if (!CanMoveTo(targetPosition)) return false;

            var originTile = GetTile(entity.GridPosition);
            var targetTile = GetTile(targetPosition);

            if (!targetTile.TrySetOccupant(entity)) return false;

            originTile?.ClearOccupant();
            entity.SetGridPosition(targetPosition);
            return true;
        }

        public bool TryPlaceEntity(IGridEntity entity, Vector2Int position)
        {
            if (!IsWithinBounds(position)) return false;

            var tile = GetTile(position);
            if (tile == null || !tile.IsWalkable) return false;

            return tile.TrySetOccupant(entity);
        }

        public void RemoveEntity(IGridEntity entity)
        {
            var tile = GetTile(entity.GridPosition);
            tile?.ClearOccupant();
        }

        public List<Vector2Int> GetNeighbors(Vector2Int position, bool onlyWalkable = false)
        {
            var neighbors = new List<Vector2Int>(4);
            Vector2Int[] directions = {
                Vector2Int.up, Vector2Int.down,
                Vector2Int.left, Vector2Int.right
            };

            foreach (var dir in directions)
            {
                var neighbor = position + dir;
                if (!IsWithinBounds(neighbor)) continue;

                if (onlyWalkable)
                {
                    var tile = GetTile(neighbor);
                    if (tile == null || !tile.IsWalkable) continue;
                }

                neighbors.Add(neighbor);
            }

            return neighbors;
        }

        public void Clear()
        {
            _tiles.Clear();
        }
    }
}
