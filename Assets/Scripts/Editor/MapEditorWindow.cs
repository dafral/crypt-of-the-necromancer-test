using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Dafral.Game.Map.Editor
{
    public partial class MapEditorWindow : EditorWindow
    {
        private enum BrushMode { Paint, Erase, Line, Rectangle, Bucket }

        [SerializeField] private LevelConfiguration _levelConfiguration;
        [SerializeField] private Vector2Int _gridSize = new(10, 10);
        [SerializeField] private float _cellSize = 1f;
        [SerializeField] private float _rhythmTempo = 120f;

        private TileConfiguration[] _palette;
        private int _selectedPaletteIndex;
        private BrushMode _brushMode = BrushMode.Paint;
        private Dictionary<Vector2Int, TileConfiguration> _tiles = new();
        private Vector2 _paletteScroll;

        private bool _isPainting;
        private bool _isDragging;
        private Vector2Int _dragStart;
        private Vector2Int _dragCurrent;

        private bool _hasHover;
        private Vector2Int _hoverCell;

        private const int MaxUndoSteps = 100;
        private readonly List<Dictionary<Vector2Int, TileConfiguration>> _undoStack = new();
        private readonly List<Dictionary<Vector2Int, TileConfiguration>> _redoStack = new();

        private MapConfiguration MapConfig =>
            _levelConfiguration != null ? _levelConfiguration.MapConfiguration : null;

        [MenuItem("Dafral/Level Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<MapEditorWindow>("Level Editor");
            window.minSize = new Vector2(300, 460);
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
            DrawLevelSettingsSection();
            EditorGUILayout.Space(8);

            using (new EditorGUI.DisabledScope(MapConfig == null))
            {
                DrawGridSettingsSection();
                EditorGUILayout.Space(8);
                DrawToolsSection();
                EditorGUILayout.Space(8);
                DrawPaletteSection();
                EditorGUILayout.Space(8);
            }

            DrawActionsSection();
        }
    }
}
