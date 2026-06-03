using System.Collections.Generic;
using System.Linq;
using Dafral.Game;
using UnityEditor;
using UnityEngine;

namespace Dafral.Game.Map.Editor
{
    public partial class MapEditorWindow
    {
        private void RefreshPalette()
        {
            var guids = AssetDatabase.FindAssets("t:TileConfiguration", new[] { "Assets/Configurations/Tiles" });
            _palette = guids
                .Select(g => AssetDatabase.LoadAssetAtPath<TileConfiguration>(AssetDatabase.GUIDToAssetPath(g)))
                .Where(t => t != null)
                .ToArray();
        }

        private void LoadFromAsset()
        {
            _tiles.Clear();
            _undoStack.Clear();
            _redoStack.Clear();

            if (_levelConfiguration == null) return;

            var tempo = _levelConfiguration.LevelData.RhythmTempo;
            _rhythmTempo = tempo > 0f ? tempo : 120f;

            var map = _levelConfiguration.MapConfiguration;
            if (map == null) return;

            _gridSize = map.GridSize;
            _cellSize = map.CellSize;

            if (map.Tiles != null)
            {
                foreach (var entry in map.Tiles)
                {
                    if (entry.TileType != null)
                        _tiles[entry.Position] = entry.TileType;
                }
            }
        }

        private void SaveToAsset()
        {
            if (_levelConfiguration == null) return;

            Undo.RecordObject(_levelConfiguration, "Save Level");
            _levelConfiguration.SetLevelData(new LevelData { RhythmTempo = _rhythmTempo });
            EditorUtility.SetDirty(_levelConfiguration);

            var map = _levelConfiguration.MapConfiguration;
            if (map != null)
            {
                Undo.RecordObject(map, "Save Level Map");

                var entries = _tiles
                    .Where(kvp => kvp.Value != null && InBounds(kvp.Key))
                    .Select(kvp => new MapConfiguration.TileEntry
                    {
                        Position = kvp.Key,
                        TileType = kvp.Value
                    })
                    .ToArray();

                map.SetData(_gridSize, _cellSize, entries);
                EditorUtility.SetDirty(map);
            }

            AssetDatabase.SaveAssets();

            Debug.Log($"[Level Editor] Saved '{_levelConfiguration.name}' " +
                      $"(tempo {_rhythmTempo} BPM, {_tiles.Count} tiles).");
        }

        private void FillAll()
        {
            var brush = GetCurrentBrush();
            if (brush == null) return;

            PushUndoState();

            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    _tiles[new Vector2Int(x, y)] = brush;
                }
            }
        }

        private void ClearAll()
        {
            if (_tiles.Count == 0) return;

            PushUndoState();
            _tiles.Clear();
        }

        private void BucketFill(Vector2Int origin)
        {
            var brush = GetCurrentBrush();
            if (brush == null || !InBounds(origin)) return;

            _tiles.TryGetValue(origin, out var target);
            if (target == brush) return;

            PushUndoState();

            var pending = new Stack<Vector2Int>();
            var visited = new HashSet<Vector2Int>();
            pending.Push(origin);

            while (pending.Count > 0)
            {
                var cell = pending.Pop();
                if (!InBounds(cell) || !visited.Add(cell)) continue;

                _tiles.TryGetValue(cell, out var current);
                if (current != target) continue;

                _tiles[cell] = brush;

                pending.Push(cell + Vector2Int.up);
                pending.Push(cell + Vector2Int.down);
                pending.Push(cell + Vector2Int.left);
                pending.Push(cell + Vector2Int.right);
            }
        }

        private TileConfiguration GetCurrentBrush()
        {
            if (_palette == null || _palette.Length == 0) return null;
            _selectedPaletteIndex = Mathf.Clamp(_selectedPaletteIndex, 0, _palette.Length - 1);
            return _palette[_selectedPaletteIndex];
        }

        private void PushUndoState()
        {
            _undoStack.Add(new Dictionary<Vector2Int, TileConfiguration>(_tiles));
            if (_undoStack.Count > MaxUndoSteps)
                _undoStack.RemoveAt(0);

            _redoStack.Clear();
        }

        private void PerformUndo()
        {
            if (_undoStack.Count == 0) return;

            _redoStack.Add(new Dictionary<Vector2Int, TileConfiguration>(_tiles));

            int last = _undoStack.Count - 1;
            _tiles = _undoStack[last];
            _undoStack.RemoveAt(last);

            SceneView.RepaintAll();
            Repaint();
        }

        private void PerformRedo()
        {
            if (_redoStack.Count == 0) return;

            _undoStack.Add(new Dictionary<Vector2Int, TileConfiguration>(_tiles));

            int last = _redoStack.Count - 1;
            _tiles = _redoStack[last];
            _redoStack.RemoveAt(last);

            SceneView.RepaintAll();
            Repaint();
        }
    }
}
