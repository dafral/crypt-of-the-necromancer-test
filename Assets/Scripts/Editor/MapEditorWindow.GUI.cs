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
            _levelConfiguration = (LevelConfiguration)EditorGUILayout.ObjectField(
                "Level Config", _levelConfiguration, typeof(LevelConfiguration), false);
            if (EditorGUI.EndChangeCheck())
            {
                LoadFromAsset();
                SceneView.RepaintAll();
            }

            if (_levelConfiguration != null && _levelConfiguration.MapConfiguration == null)
            {
                EditorGUILayout.HelpBox(
                    "This LevelConfiguration has no MapConfiguration assigned. " +
                    "Tile editing is disabled until you assign one on the asset.",
                    MessageType.Warning);
            }
        }

        private void DrawLevelSettingsSection()
        {
            EditorGUILayout.LabelField("Level Settings", EditorStyles.miniBoldLabel);

            using (new EditorGUI.DisabledScope(_levelConfiguration == null))
            {
                _rhythmTempo = EditorGUILayout.FloatField("Rhythm Tempo (BPM)", _rhythmTempo);
                _rhythmTempo = Mathf.Clamp(_rhythmTempo, 1f, 1000f);
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
            EditorGUILayout.LabelField("Tools", EditorStyles.miniBoldLabel);

            EditorGUILayout.BeginHorizontal();
            DrawToolToggle(BrushMode.Paint, "Paint (B)");
            DrawToolToggle(BrushMode.Erase, "Erase (X)");
            DrawToolToggle(BrushMode.Line, "Line (L)");
            DrawToolToggle(BrushMode.Rectangle, "Rect (K)");
            DrawToolToggle(BrushMode.Bucket, "Fill (G)");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.HelpBox(
                "Alt+Click = eyedropper  |  1-9 = select palette  |  Ctrl+Z / Ctrl+Y = undo / redo",
                MessageType.None);
        }

        private void DrawToolToggle(BrushMode mode, string label)
        {
            bool isActive = _brushMode == mode;
            bool pressed = GUILayout.Toggle(isActive, label, EditorStyles.miniButton);
            if (pressed && !isActive)
                _brushMode = mode;
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

            _paletteScroll = EditorGUILayout.BeginScrollView(_paletteScroll, GUILayout.MaxHeight(200));

            for (int i = 0; i < _palette.Length; i++)
            {
                var tile = _palette[i];
                if (tile == null) continue;

                EditorGUILayout.BeginHorizontal();

                var previewRect = GUILayoutUtility.GetRect(20, 20, GUILayout.Width(20), GUILayout.Height(20));
                var previewSprite = tile.GetPreviewSprite();
                if (previewSprite != null)
                    DrawSpritePreview(previewRect, previewSprite, Color.white);
                else
                    EditorGUI.DrawRect(previewRect, tile.EditorColor);

                var label = tile.VariantCount > 1 ? $"{tile.name}  x{tile.VariantCount}" : tile.name;
                var style = _selectedPaletteIndex == i ? EditorStyles.boldLabel : EditorStyles.label;
                if (GUILayout.Button(label, style))
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
            using (new EditorGUI.DisabledScope(_undoStack.Count == 0))
            {
                if (GUILayout.Button("Undo"))
                    PerformUndo();
            }
            using (new EditorGUI.DisabledScope(_redoStack.Count == 0))
            {
                if (GUILayout.Button("Redo"))
                    PerformRedo();
            }
            EditorGUILayout.EndHorizontal();

            using (new EditorGUI.DisabledScope(MapConfig == null))
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Fill All"))
                {
                    FillAll();
                    SceneView.RepaintAll();
                }
                if (GUILayout.Button("Clear All"))
                {
                    ClearAll();
                    SceneView.RepaintAll();
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(4);

            using (new EditorGUI.DisabledScope(_levelConfiguration == null))
            {
                if (GUILayout.Button("Save to Asset", GUILayout.Height(28)))
                    SaveToAsset();
            }

            if (_levelConfiguration == null)
            {
                EditorGUILayout.HelpBox(
                    "Assign a LevelConfiguration asset to save.",
                    MessageType.Warning);
            }
        }

        private static void DrawSpritePreview(Rect rect, Sprite sprite, Color tint)
        {
            if (sprite == null || sprite.texture == null)
                return;

            var texture = sprite.texture;
            var tr = sprite.textureRect;
            var texCoords = new Rect(
                tr.x / texture.width,
                tr.y / texture.height,
                tr.width / texture.width,
                tr.height / texture.height);

            var previous = GUI.color;
            GUI.color = tint;
            GUI.DrawTextureWithTexCoords(rect, texture, texCoords, true);
            GUI.color = previous;
        }
    }
}
