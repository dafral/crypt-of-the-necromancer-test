using System.Collections.Generic;
using Dafral.Grid;
using UnityEditor;
using UnityEngine;

namespace Dafral.Grid.Editor
{
    public partial class LevelEditorWindow : EditorWindow
    {
        private enum BrushMode { Paint, Erase }

        [SerializeField] private LevelDataConfiguration _levelData;
        [SerializeField] private Vector2Int _gridSize = new(10, 10);
        [SerializeField] private float _cellSize = 1f;

        private TileConfiguration[] _palette;
        private int _selectedPaletteIndex;
        private BrushMode _brushMode = BrushMode.Paint;
        private Dictionary<Vector2Int, TileConfiguration> _tiles = new();
        private Vector2 _paletteScroll;
        private bool _isPainting;

        [MenuItem("Dafral/Level Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<LevelEditorWindow>("Level Editor");
            window.minSize = new Vector2(280, 400);
        }

        private void OnEnable()
        {
            SceneView.duringSceneGui += OnSceneGUI;
            RefreshPalette();
            LoadFromAsset();
        }

        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneGUI;
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Level Editor", EditorStyles.boldLabel);
            EditorGUILayout.Space(4);

            DrawLevelAssetSection();
            EditorGUILayout.Space(8);
            DrawGridSettingsSection();
            EditorGUILayout.Space(8);
            DrawToolsSection();
            EditorGUILayout.Space(8);
            DrawPaletteSection();
            EditorGUILayout.Space(8);
            DrawActionsSection();
        }
    }
}
