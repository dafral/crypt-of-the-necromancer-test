using System.Linq;
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
            if (_mapConfiguration == null) return;

            _gridSize = _mapConfiguration.GridSize;
            _cellSize = _mapConfiguration.CellSize;

            if (_mapConfiguration.Tiles != null)
            {
                foreach (var entry in _mapConfiguration.Tiles)
                {
                    if (entry.TileType != null)
                        _tiles[entry.Position] = entry.TileType;
                }
            }
        }

        private void SaveToAsset()
        {
            if (_mapConfiguration == null) return;

            Undo.RecordObject(_mapConfiguration, "Save Level Data");

            var entries = _tiles
                .Select(kvp => new MapConfiguration.TileEntry
                {
                    Position = kvp.Key,
                    TileType = kvp.Value
                })
                .ToArray();

            _mapConfiguration.SetData(_gridSize, _cellSize, entries);
            EditorUtility.SetDirty(_mapConfiguration);
            AssetDatabase.SaveAssets();

            Debug.Log($"[Level Editor] Saved {entries.Length} tiles to {AssetDatabase.GetAssetPath(_mapConfiguration)}");
        }

        private void FillAll()
        {
            var brush = GetCurrentBrush();
            if (brush == null) return;

            if (_mapConfiguration != null)
                Undo.RecordObject(_mapConfiguration, "Fill All Tiles");

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
