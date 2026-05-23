using System.Linq;
using Dafral.Grid;
using UnityEditor;
using UnityEngine;

namespace Dafral.Grid.Editor
{
    public partial class LevelEditorWindow
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
            if (_levelData == null) return;

            _gridSize = _levelData.GridSize;
            _cellSize = _levelData.CellSize;

            if (_levelData.Tiles != null)
            {
                foreach (var entry in _levelData.Tiles)
                {
                    if (entry.TileType != null)
                        _tiles[entry.Position] = entry.TileType;
                }
            }
        }

        private void SaveToAsset()
        {
            if (_levelData == null) return;

            Undo.RecordObject(_levelData, "Save Level Data");

            var entries = _tiles
                .Select(kvp => new LevelDataConfiguration.TileEntry
                {
                    Position = kvp.Key,
                    TileType = kvp.Value
                })
                .ToArray();

            _levelData.SetData(_gridSize, _cellSize, entries);
            EditorUtility.SetDirty(_levelData);
            AssetDatabase.SaveAssets();

            Debug.Log($"[Level Editor] Saved {entries.Length} tiles to {AssetDatabase.GetAssetPath(_levelData)}");
        }

        private void FillAll()
        {
            var brush = GetCurrentBrush();
            if (brush == null) return;

            if (_levelData != null)
                Undo.RecordObject(_levelData, "Fill All Tiles");

            for (int x = 0; x < _gridSize.x; x++)
            {
                for (int y = 0; y < _gridSize.y; y++)
                {
                    _tiles[new Vector2Int(x, y)] = brush;
                }
            }
        }

        private TileConfiguration GetCurrentBrush()
        {
            if (_palette == null || _palette.Length == 0) return null;
            _selectedPaletteIndex = Mathf.Clamp(_selectedPaletteIndex, 0, _palette.Length - 1);
            return _palette[_selectedPaletteIndex];
        }
    }
}
