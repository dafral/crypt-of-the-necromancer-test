using UnityEngine;

namespace Dafral.Grid
{
    [CreateAssetMenu(fileName = "NewTileConfiguration", menuName = "Dafral/Map/Tile Configuration")]
    public class TileConfiguration : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string _id;

        [Header("Gameplay")]
        [SerializeField] private bool _isWalkable = true;

        [Header("Visuals")]
        [SerializeField] private Sprite _sprite;
        [SerializeField] private Color _editorColor = Color.white;
        [SerializeField] private GameObject _prefab;

        public string Id => _id;
        public bool IsWalkable => _isWalkable;
        public Sprite Sprite => _sprite;
        public Color EditorColor => _editorColor;
        public GameObject Prefab => _prefab;
    }
}
