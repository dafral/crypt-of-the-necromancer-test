using System.Collections.Generic;
using System.IO;
using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.Map
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

            LogAdjacentTileGaps();
        }

        private void CreateTileVisual(Vector2Int gridPosition, TileData tileData)
        {
            var worldPos = _coordinateConverter.GridToWorld(gridPosition);
            GameObject tileObject;

            if (tileData.TileType.Prefab != null)
            {
                tileObject = Instantiate(tileData.TileType.Prefab, worldPos, Quaternion.identity, GetParent());
                var spriteRenderer = tileObject.GetComponent<SpriteRenderer>();

                if (spriteRenderer == null && IsSpawnTile(tileData.TileType))
                {
                    spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
                }

                ApplyCellSprite(tileObject, spriteRenderer, tileData.TileType, gridPosition);
            }
            else
            {
                tileObject = CreateDefaultTileVisual(worldPos, tileData.TileType, gridPosition);
            }

            tileObject.name = $"Tile_{gridPosition.x}_{gridPosition.y}";
            _tileVisuals[gridPosition] = tileObject;
        }

        private GameObject CreateDefaultTileVisual(Vector3 worldPos, TileConfiguration tileType, Vector2Int gridPosition)
        {
            var tileObject = new GameObject();
            tileObject.transform.SetParent(GetParent());
            tileObject.transform.position = worldPos;

            var spriteRenderer = tileObject.AddComponent<SpriteRenderer>();
            ApplyCellSprite(tileObject, spriteRenderer, tileType, gridPosition);

            return tileObject;
        }

        private void ApplyCellSprite(GameObject tileObject, SpriteRenderer spriteRenderer, TileConfiguration tileType, Vector2Int gridPosition)
        {
            if (spriteRenderer == null)
            {
                return;
            }

            var sprite = tileType.GetSpriteForCell(gridPosition);
            spriteRenderer.sprite = sprite;
            spriteRenderer.color = sprite != null ? Color.white : tileType.EditorColor;

            var scale = GetScaleForCell(sprite);
            tileObject.transform.localScale = new Vector3(scale, scale, 1f);

            // #region agent log
            if (gridPosition.x <= 18 && gridPosition.y == 7)
            {
                var expectedWorldSize = sprite != null
                    ? Mathf.Max(sprite.rect.width, sprite.rect.height) / sprite.pixelsPerUnit
                    : 0f;
                DbgLog(
                    scale < 0.999f ? "A" : "E",
                    "GridVisualizer.ApplyCellSprite",
                    "tile scale and sprite sizing",
                    $"{{\"gridX\":{gridPosition.x},\"gridY\":{gridPosition.y},\"cellSize\":{_coordinateConverter.CellSize},\"scale\":{scale:F6},\"expectedWorldSize\":{expectedWorldSize:F6},\"boundsX\":{(sprite != null ? sprite.bounds.size.x : 0f):F6},\"boundsY\":{(sprite != null ? sprite.bounds.size.y : 0f):F6},\"spriteName\":\"{(sprite != null ? sprite.name : "null")}\"}}");
            }
            // #endregion
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

        private static bool IsSpawnTile(TileConfiguration tileType)
        {
            return tileType != null && tileType.Id != null && tileType.Id.Contains("spawn");
        }

        // #region agent log
        private void LogAdjacentTileGaps()
        {
            for (int x = 16; x <= 20; x++)
            {
                var left = new Vector2Int(x, 7);
                var right = new Vector2Int(x + 1, 7);
                if (!_tileVisuals.TryGetValue(left, out var leftTile) || !_tileVisuals.TryGetValue(right, out var rightTile))
                {
                    continue;
                }

                var leftRenderer = leftTile.GetComponent<SpriteRenderer>();
                var rightRenderer = rightTile.GetComponent<SpriteRenderer>();
                if (leftRenderer == null || rightRenderer == null)
                {
                    continue;
                }

                var leftBounds = leftRenderer.bounds;
                var rightBounds = rightRenderer.bounds;
                var horizontalGap = rightBounds.min.x - leftBounds.max.x;
                var centerDistance = Vector3.Distance(leftTile.transform.position, rightTile.transform.position);

                DbgLog(
                    horizontalGap > 0.0001f ? "B" : "D",
                    "GridVisualizer.LogAdjacentTileGaps",
                    "adjacent tile seam measurement",
                    $"{{\"leftX\":{left.x},\"rightX\":{right.x},\"horizontalGap\":{horizontalGap:F6},\"centerDistance\":{centerDistance:F6},\"leftMaxX\":{leftBounds.max.x:F6},\"rightMinX\":{rightBounds.min.x:F6},\"leftScale\":{leftTile.transform.localScale.x:F6}}}");
            }
        }

        private static void DbgLog(string hypothesisId, string location, string message, string dataJson)
        {
            try
            {
                var path = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "debug-7de642.log");
                var timestamp = System.DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                File.AppendAllText(
                    path,
                    $"{{\"sessionId\":\"7de642\",\"hypothesisId\":\"{hypothesisId}\",\"location\":\"{location}\",\"message\":\"{message}\",\"data\":{dataJson},\"timestamp\":{timestamp}}}\n");
            }
            catch
            {
            }
        }
        // #endregion
    }
}
