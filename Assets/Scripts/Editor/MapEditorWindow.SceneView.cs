using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dafral.Game.Map.Editor
{
    public partial class MapEditorWindow
    {
        private void OnSceneGUI(SceneView sceneView)
        {
            if (MapConfig == null) return;

            DrawGrid();
            DrawTiles();
            DrawDragPreview();
            DrawHover();
            HandleInput(sceneView);
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
        }

        private void DrawTiles()
        {
            Handles.BeginGUI();
            foreach (var kvp in _tiles)
            {
                var tile = kvp.Value;
                if (tile == null) continue;

                var rect = GetCellGUIRect(kvp.Key);
                var sprite = tile.GetSpriteForCell(kvp.Key);

                if (sprite != null)
                    DrawSpritePreview(rect, sprite, Color.white);
                else
                    EditorGUI.DrawRect(rect, tile.EditorColor);
            }
            Handles.EndGUI();
        }

        private void DrawDragPreview()
        {
            if (!_isDragging) return;

            var brush = GetCurrentBrush();
            var color = brush != null
                ? new Color(brush.EditorColor.r, brush.EditorColor.g, brush.EditorColor.b, 0.35f)
                : new Color(1f, 1f, 1f, 0.2f);

            foreach (var cell in GetToolCells(_brushMode, _dragStart, _dragCurrent))
            {
                if (!InBounds(cell)) continue;
                DrawCellQuad(cell, color, new Color(1f, 1f, 1f, 0.6f));
            }
        }

        private void DrawHover()
        {
            if (!_hasHover || !InBounds(_hoverCell)) return;

            var fill = _brushMode == BrushMode.Erase
                ? new Color(1f, 0.2f, 0.2f, 0.15f)
                : new Color(1f, 1f, 1f, 0.12f);

            DrawCellQuad(_hoverCell, fill, new Color(1f, 1f, 1f, 0.8f));

            var labelPos = GridToWorld(_hoverCell) + new Vector3(-_cellSize * 0.5f, _cellSize * 0.55f, 0f);
            Handles.Label(labelPos, $"{_hoverCell.x}, {_hoverCell.y}");
        }

        private void DrawCellQuad(Vector2Int cell, Color fill, Color outline)
        {
            var center = GridToWorld(cell);
            var half = _cellSize * 0.5f;

            var verts = new Vector3[]
            {
                center + new Vector3(-half, -half, 0f),
                center + new Vector3(-half, half, 0f),
                center + new Vector3(half, half, 0f),
                center + new Vector3(half, -half, 0f),
            };

            Handles.DrawSolidRectangleWithOutline(verts, fill, outline);
        }

        private void HandleInput(SceneView sceneView)
        {
            Event e = Event.current;
            int controlId = GUIUtility.GetControlID(FocusType.Passive);

            if (e.type == EventType.Layout)
            {
                HandleUtility.AddDefaultControl(controlId);
                return;
            }

            HandleKeyboard(e);

            bool hasCell = TryGetCellUnderMouse(e, out var cell);
            if (hasCell)
            {
                if (!_hasHover || cell != _hoverCell)
                {
                    _hoverCell = cell;
                    _hasHover = true;
                    sceneView.Repaint();
                }
            }
            else
            {
                _hasHover = false;
            }

            if (e.button != 0) return;

            switch (e.type)
            {
                case EventType.MouseDown:
                    OnMouseDown(e, cell, hasCell);
                    e.Use();
                    break;
                case EventType.MouseDrag:
                    OnMouseDrag(e, cell, hasCell, sceneView);
                    e.Use();
                    break;
                case EventType.MouseUp:
                    OnMouseUp(cell, hasCell);
                    e.Use();
                    break;
            }
        }

        private void OnMouseDown(Event e, Vector2Int cell, bool hasCell)
        {
            if (!hasCell) return;

            if (e.alt)
            {
                TryEyedropper(cell);
                return;
            }

            switch (_brushMode)
            {
                case BrushMode.Paint:
                case BrushMode.Erase:
                    PushUndoState();
                    _isPainting = true;
                    ApplyBrush(cell);
                    break;

                case BrushMode.Line:
                case BrushMode.Rectangle:
                    _isDragging = true;
                    _dragStart = cell;
                    _dragCurrent = cell;
                    break;

                case BrushMode.Bucket:
                    BucketFill(cell);
                    break;
            }

            SceneView.RepaintAll();
            Repaint();
        }

        private void OnMouseDrag(Event e, Vector2Int cell, bool hasCell, SceneView sceneView)
        {
            if (!hasCell) return;

            if (_isPainting)
            {
                ApplyBrush(cell);
                SceneView.RepaintAll();
            }
            else if (_isDragging)
            {
                _dragCurrent = cell;
                sceneView.Repaint();
            }
        }

        private void OnMouseUp(Vector2Int cell, bool hasCell)
        {
            if (_isPainting)
            {
                _isPainting = false;
            }
            else if (_isDragging)
            {
                if (hasCell) _dragCurrent = cell;
                CommitDrag();
                _isDragging = false;
            }

            SceneView.RepaintAll();
            Repaint();
        }

        private void CommitDrag()
        {
            var brush = GetCurrentBrush();
            if (brush == null) return;

            PushUndoState();

            foreach (var cell in GetToolCells(_brushMode, _dragStart, _dragCurrent))
            {
                if (InBounds(cell))
                    _tiles[cell] = brush;
            }
        }

        private void ApplyBrush(Vector2Int cell)
        {
            if (!InBounds(cell)) return;

            if (_brushMode == BrushMode.Erase)
            {
                _tiles.Remove(cell);
            }
            else
            {
                var brush = GetCurrentBrush();
                if (brush != null)
                    _tiles[cell] = brush;
            }
        }

        private void TryEyedropper(Vector2Int cell)
        {
            if (!_tiles.TryGetValue(cell, out var tile) || tile == null || _palette == null)
                return;

            int index = System.Array.IndexOf(_palette, tile);
            if (index >= 0)
            {
                _selectedPaletteIndex = index;
                _brushMode = BrushMode.Paint;
                Repaint();
            }
        }

        private void HandleKeyboard(Event e)
        {
            if (e.type != EventType.KeyDown) return;

            if (e.control || e.command)
            {
                if (e.keyCode == KeyCode.Z) { PerformUndo(); e.Use(); return; }
                if (e.keyCode == KeyCode.Y) { PerformRedo(); e.Use(); return; }
            }

            if (e.keyCode >= KeyCode.Alpha1 && e.keyCode <= KeyCode.Alpha9)
            {
                int index = e.keyCode - KeyCode.Alpha1;
                if (_palette != null && index < _palette.Length)
                {
                    _selectedPaletteIndex = index;
                    _brushMode = BrushMode.Paint;
                    e.Use();
                    Repaint();
                }
                return;
            }

            switch (e.keyCode)
            {
                case KeyCode.B: _brushMode = BrushMode.Paint; e.Use(); Repaint(); break;
                case KeyCode.X: _brushMode = BrushMode.Erase; e.Use(); Repaint(); break;
                case KeyCode.L: _brushMode = BrushMode.Line; e.Use(); Repaint(); break;
                case KeyCode.K: _brushMode = BrushMode.Rectangle; e.Use(); Repaint(); break;
                case KeyCode.G: _brushMode = BrushMode.Bucket; e.Use(); Repaint(); break;
            }
        }

        private static IEnumerable<Vector2Int> GetToolCells(BrushMode mode, Vector2Int start, Vector2Int end)
        {
            return mode == BrushMode.Line ? GetLineCells(start, end) : GetRectCells(start, end);
        }

        private static IEnumerable<Vector2Int> GetRectCells(Vector2Int a, Vector2Int b)
        {
            int minX = Mathf.Min(a.x, b.x);
            int maxX = Mathf.Max(a.x, b.x);
            int minY = Mathf.Min(a.y, b.y);
            int maxY = Mathf.Max(a.y, b.y);

            for (int x = minX; x <= maxX; x++)
                for (int y = minY; y <= maxY; y++)
                    yield return new Vector2Int(x, y);
        }

        private static IEnumerable<Vector2Int> GetLineCells(Vector2Int a, Vector2Int b)
        {
            int x0 = a.x, y0 = a.y;
            int x1 = b.x, y1 = b.y;

            int dx = Mathf.Abs(x1 - x0);
            int dy = -Mathf.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx + dy;

            while (true)
            {
                yield return new Vector2Int(x0, y0);
                if (x0 == x1 && y0 == y1) break;

                int e2 = 2 * err;
                if (e2 >= dy) { err += dy; x0 += sx; }
                if (e2 <= dx) { err += dx; y0 += sy; }
            }
        }

        private bool TryGetCellUnderMouse(Event e, out Vector2Int cell)
        {
            cell = default;

            var ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Mathf.Approximately(ray.direction.z, 0f)) return false;

            float t = -ray.origin.z / ray.direction.z;
            var worldPoint = ray.origin + ray.direction * t;
            cell = WorldToGrid(worldPoint);
            return true;
        }

        private bool InBounds(Vector2Int cell)
        {
            return cell.x >= 0 && cell.x < _gridSize.x
                && cell.y >= 0 && cell.y < _gridSize.y;
        }

        private Rect GetCellGUIRect(Vector2Int cell)
        {
            var center = GridToWorld(cell);
            var half = _cellSize * 0.5f;

            var topLeft = HandleUtility.WorldToGUIPoint(center + new Vector3(-half, half, 0f));
            var bottomRight = HandleUtility.WorldToGUIPoint(center + new Vector3(half, -half, 0f));

            return new Rect(topLeft.x, topLeft.y, bottomRight.x - topLeft.x, bottomRight.y - topLeft.y);
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
