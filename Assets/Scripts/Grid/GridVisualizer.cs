using System.Collections.Generic;
using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Grid
{
    public class GridVisualizer : MonoBehaviour
    {
        [SerializeField] private Transform _tileParent;

        private readonly Dictionary<Vector2Int, GameObject> _tileVisuals = new();
        private GridCoordinateConverter _coordinateConverter;

        private void OnEnable()
        {
            if (!ServiceLocator.Instance.Contains<IEventService>())
            {
                return;
            }

            var eventService = ServiceLocator.Instance.GetService<IEventService>();
            eventService.Subscribe<OnGridLoaded>(OnGridLoaded);

            TrySyncWithLoadedGrid();
        }

        private void OnDisable()
        {
            if (!ServiceLocator.Instance.Contains<IEventService>())
            {
                return;
            }

            var eventService = ServiceLocator.Instance.GetService<IEventService>();
            eventService.Unsubscribe<OnGridLoaded>(OnGridLoaded);
        }

        private void TrySyncWithLoadedGrid()
        {
            if (!ServiceLocator.Instance.Contains<IGridService>())
            {
                return;
            }

            var gridService = ServiceLocator.Instance.GetService<IGridService>();
            if (gridService.Grid == null)
            {
                return;
            }

            _coordinateConverter = gridService.CoordinateConverter;
            RebuildVisuals(gridService.Grid);
        }

        private void OnGridLoaded(OnGridLoaded e)
        {
            _coordinateConverter = e.CoordinateConverter;
            RebuildVisuals(e.Grid);
        }

        public void RebuildVisuals(GridData grid)
        {
            ClearVisuals();

            foreach (var kvp in grid.Tiles)
            {
                CreateTileVisual(kvp.Key, kvp.Value);
            }
        }

        private void CreateTileVisual(Vector2Int gridPosition, TileData tileData)
        {
            if (tileData.TileType == null) return;

            var worldPos = _coordinateConverter.GridToWorld(gridPosition);
            var tileObject = CreateDefaultTileVisual(worldPos, tileData.TileType);

            tileObject.name = $"Tile_{gridPosition.x}_{gridPosition.y}";
            _tileVisuals[gridPosition] = tileObject;
        }

        private GameObject CreateDefaultTileVisual(Vector3 worldPos, TileConfiguration tileType)
        {
            var tileObject = new GameObject();
            tileObject.transform.SetParent(GetParent());
            tileObject.transform.position = worldPos;

            var spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = tileType.Sprite;
            spriteRenderer.color = tileType.Sprite != null ? Color.white : tileType.EditorColor;

            var scale = GetScaleForCell(tileType.Sprite);
            tileObject.transform.localScale = new Vector3(scale, scale, 1f);

            return tileObject;
        }

        public void UpdateTileVisual(Vector2Int position, TileData tileData)
        {
            if (_tileVisuals.TryGetValue(position, out var existing))
            {
                Destroy(existing);
                _tileVisuals.Remove(position);
            }

            CreateTileVisual(position, tileData);
        }

        private void ClearVisuals()
        {
            foreach (var visual in _tileVisuals.Values)
            {
                Destroy(visual);
            }
            _tileVisuals.Clear();
        }

        private float GetScaleForCell(Sprite sprite)
        {
            if (sprite == null)
            {
                return _coordinateConverter.CellSize;
            }

            var spriteWorldSize = Mathf.Max(sprite.bounds.size.x, sprite.bounds.size.y);
            if (spriteWorldSize <= 0f)
            {
                return 1f;
            }

            return _coordinateConverter.CellSize / spriteWorldSize;
        }

        private Transform GetParent()
        {
            return _tileParent != null ? _tileParent : transform;
        }
    }
}
