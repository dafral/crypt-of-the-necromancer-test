using System;
using UnityEngine;

namespace Dafral.Game.Map
{
    [CreateAssetMenu(fileName = "NewMapConfiguration", menuName = "Dafral/Game/Map Configuration")]
    public class MapConfiguration : ScriptableObject
    {
        [SerializeField] private Vector2Int _gridSize = new(10, 10);
        [SerializeField] private float _cellSize = 1f;
        [SerializeField] private TileEntry[] _tiles;

        public Vector2Int GridSize => _gridSize;
        public float CellSize => _cellSize;
        public TileEntry[] Tiles => _tiles;

        public void SetData(Vector2Int gridSize, float cellSize, TileEntry[] tiles)
        {
            _gridSize = gridSize;
            _cellSize = cellSize;
            _tiles = tiles;
        }

        [Serializable]
        public struct TileEntry
        {
            public Vector2Int Position;
            public TileConfiguration TileType;
        }
    }
}
