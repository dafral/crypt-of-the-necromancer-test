using Dafral.Game.Map;
using UnityEditor;
using UnityEngine;

namespace Dafral.Game.Map.Editor
{
    public partial class MapEditorWindow
    {
        private void DrawLevelAssetSection()
        {
            EditorGUILayout.LabelField("Target Level", EditorStyles.miniBoldLabel);

            EditorGUI.BeginChangeCheck();
            _mapConfiguration = (MapConfiguration)EditorGUILayout.ObjectField(
                "Level Data", _mapConfiguration, typeof(MapConfiguration), false);
            if (EditorGUI.EndChangeCheck())
            {
                LoadFromAsset();
                SceneView.RepaintAll();
            }
        }

        private void DrawGridSettingsSection()
        {
            EditorGUILayout.LabelField("Grid Settings", EditorStyles.miniBoldLabel);

            _gridSize = EditorGUILayout.Vector2IntField("Grid Size", _gridSize);
            _gridSize = Vector2Int.Max(_gridSize, Vector2Int.one);

            _cellSize = EditorGUILayout.FloatField("Cell Size", _cellSize);
            _cellSize = Mathf.Max(_cellSize, 0.1f);

            if (_cellSize != 1f)
            {
                EditorGUILayout.HelpBox(
                    "cellSize != 1 may cause misalignment with 16 PPU sprites.",
                    MessageType.Warning);
            }
        }

        private void DrawToolsSection()
        {
            EditorGUILayout.LabelField("Brush", EditorStyles.miniBoldLabel);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Toggle(_brushMode == BrushMode.Paint, "Paint", EditorStyles.miniButtonLeft))
                _brushMode = BrushMode.Paint;
            if (GUILayout.Toggle(_brushMode == BrushMode.Erase, "Erase", EditorStyles.miniButtonRight))
                _brushMode = BrushMode.Erase;
            EditorGUILayout.EndHorizontal();
        }

        private void DrawPaletteSection()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Palette", EditorStyles.miniBoldLabel);
            if (GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.Width(60)))
                RefreshPalette();
            EditorGUILayout.EndHorizontal();

            if (_palette == null || _palette.Length == 0)
            {
                EditorGUILayout.HelpBox(
                    "No TileConfiguration assets found in Assets/Configurations/Tiles/",
                    MessageType.Info);
                return;
            }

            _paletteScroll = EditorGUILayout.BeginScrollView(_paletteScroll, GUILayout.MaxHeight(160));

            for (int i = 0; i < _palette.Length; i++)
            {
                var tile = _palette[i];
                if (tile == null) continue;

                EditorGUILayout.BeginHorizontal();

                var colorRect = GUILayoutUtility.GetRect(16, 16, GUILayout.Width(16), GUILayout.Height(16));
                EditorGUI.DrawRect(colorRect, tile.EditorColor);

                var style = _selectedPaletteIndex == i ? EditorStyles.boldLabel : EditorStyles.label;
                if (GUILayout.Button(tile.name, style))
                {
                    _selectedPaletteIndex = i;
                    _brushMode = BrushMode.Paint;
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawActionsSection()
        {
            EditorGUILayout.LabelField("Actions", EditorStyles.miniBoldLabel);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Fill All"))
            {
                FillAll();
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("Clear All"))
            {
                if (_mapConfiguration != null)
                    Undo.RecordObject(_mapConfiguration, "Clear All Tiles");
                _tiles.Clear();
                SceneView.RepaintAll();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(4);

            GUI.enabled = _mapConfiguration != null;
            if (GUILayout.Button("Save to Asset", GUILayout.Height(30)))
                SaveToAsset();
            GUI.enabled = true;

            if (_mapConfiguration == null)
            {
                EditorGUILayout.HelpBox(
                    "Assign a LevelDataConfiguration asset to save.",
                    MessageType.Warning);
            }
        }
    }
}
