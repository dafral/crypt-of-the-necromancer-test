using UnityEditor;
using UnityEngine;

namespace Dafral.Grid.Editor
{
    public partial class LevelEditorWindow
    {
        private void OnSceneGUI(SceneView sceneView)
        {
            if (_levelData == null) return;

            DrawGrid();
            DrawTiles();
            HandleInput();
        }

        private void DrawGrid()
        {
            Handles.color = new Color(1f, 1f, 1f, 0.2f);

            for (int x = 0; x <= _gridSize.x; x++)
            {
                var start = new Vector3(x * _cellSize, 0f, 0f);
                var end = new Vector3(x * _cellSize, _gridSize.y * _cellSize, 0f);
                Handles.DrawLine(start, end);
            }

            for (int y = 0; y <= _gridSize.y; y++)
            {
                var start = new Vector3(0f, y * _cellSize, 0f);
                var end = new Vector3(_gridSize.x * _cellSize, y * _cellSize, 0f);
                Handles.DrawLine(start, end);
            }

            Handles.color = new Color(0f, 1f, 0f, 0.4f);
            var max = new Vector3(_gridSize.x * _cellSize, _gridSize.y * _cellSize, 0f);
            Handles.DrawLine(Vector3.zero, new Vector3(max.x, 0f, 0f));
            Handles.DrawLine(new Vector3(max.x, 0f, 0f), max);
            Handles.DrawLine(max, new Vector3(0f, max.y, 0f));
            Handles.DrawLine(new Vector3(0f, max.y, 0f), Vector3.zero);
        }

        private void DrawTiles()
        {
            foreach (var kvp in _tiles)
            {
                var tile = kvp.Value;
                if (tile == null) continue;

                var worldCenter = GridToWorld(kvp.Key);
                var halfSize = _cellSize * 0.45f;

                var verts = new Vector3[]
                {
                    worldCenter + new Vector3(-halfSize, -halfSize, 0f),
                    worldCenter + new Vector3(-halfSize, halfSize, 0f),
                    worldCenter + new Vector3(halfSize, halfSize, 0f),
                    worldCenter + new Vector3(halfSize, -halfSize, 0f),
                };

                Handles.DrawSolidRectangleWithOutline(verts, tile.EditorColor, Color.clear);
            }
        }

        private void HandleInput()
        {
            Event e = Event.current;
            int controlId = GUIUtility.GetControlID(FocusType.Passive);

            if (e.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(controlId);
                return;
            }

            bool isMouseDown = e.type == EventType.MouseDown && e.button == 0;
            bool isMouseDrag = e.type == EventType.MouseDrag && e.button == 0;
            bool isMouseUp = e.type == EventType.MouseUp && e.button == 0;

            if (isMouseDown)
            {
                _isPainting = true;
                PaintAtMouse(e);
                e.Use();
            }
            else if (isMouseDrag && _isPainting)
            {
                PaintAtMouse(e);
                e.Use();
            }
            else if (isMouseUp && _isPainting)
            {
                _isPainting = false;
                e.Use();
            }
        }

        private void PaintAtMouse(Event e)
        {
            var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);

            if (Mathf.Approximately(ray.direction.z, 0f)) return;

            float t = -ray.origin.z / ray.direction.z;
            var worldPoint = ray.origin + ray.direction * t;
            var gridPos = WorldToGrid(worldPoint);

            if (gridPos.x < 0 || gridPos.x >= _gridSize.x ||
                gridPos.y < 0 || gridPos.y >= _gridSize.y)
                return;

            if (_brushMode == BrushMode.Paint)
            {
                var brush = GetCurrentBrush();
                if (brush == null) return;
                _tiles[gridPos] = brush;
            }
            else
            {
                _tiles.Remove(gridPos);
            }

            SceneView.RepaintAll();
            Repaint();
        }

        private Vector3 GridToWorld(Vector2Int gridPosition)
        {
            return new Vector3(
                gridPosition.x * _cellSize + _cellSize * 0.5f,
                gridPosition.y * _cellSize + _cellSize * 0.5f,
                0f
            );
        }

        private Vector2Int WorldToGrid(Vector3 worldPosition)
        {
            return new Vector2Int(
                Mathf.FloorToInt(worldPosition.x / _cellSize),
                Mathf.FloorToInt(worldPosition.y / _cellSize)
            );
        }
    }
}
