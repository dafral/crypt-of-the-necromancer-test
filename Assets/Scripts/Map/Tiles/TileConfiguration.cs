using System;
using UnityEngine;

namespace Dafral.Game.Map
{
    [CreateAssetMenu(fileName = "NewTileConfiguration", menuName = "Dafral/Game/Tile Configuration")]
    public class TileConfiguration : ScriptableObject
    {
        [Header("Identity")]
        [SerializeField] private string _id;

        [Header("Gameplay")]
        [SerializeField] private bool _isWalkable = true;

        [Header("Visuals")]
        [SerializeField] private Sprite _sprite;
        [Tooltip("Optional extra visuals. When set, a variant is picked deterministically per cell so the same tile type can appear in many forms.")]
        [SerializeField] private SpriteVariant[] _spriteVariants;
        [SerializeField] private Color _editorColor = Color.white;
        [SerializeField] private GameObject _prefab;

        public string Id => _id;
        public virtual bool IsWalkable => _isWalkable;
        public Sprite Sprite => _sprite;
        public Color EditorColor => _editorColor;
        public GameObject Prefab => _prefab;

        /// <summary>Number of distinct sprites this tile can render (base sprite + variants).</summary>
        public int VariantCount => CountPool();

        /// <summary>A representative sprite for inspectors/palette previews.</summary>
        public Sprite GetPreviewSprite()
        {
            if (_sprite != null) return _sprite;

            if (_spriteVariants != null)
            {
                foreach (var variant in _spriteVariants)
                {
                    if (variant.Sprite != null) return variant.Sprite;
                }
            }

            return null;
        }

        /// <summary>
        /// Picks a sprite for a given grid cell. The choice is weighted and deterministic
        /// (the same position always yields the same sprite), so the look is stable across reloads.
        /// </summary>
        public Sprite GetSpriteForCell(Vector2Int position)
        {
            int count = CountPool();
            if (count == 0) return null;
            if (count == 1) return GetPreviewSprite();

            float total = TotalWeight();
            if (total <= 0f) return GetPreviewSprite();

            var rng = CreateDeterministicRng(position);
            double roll = rng.NextDouble() * total;

            if (_sprite != null)
            {
                roll -= 1f;
                if (roll <= 0d) return _sprite;
            }

            if (_spriteVariants != null)
            {
                foreach (var variant in _spriteVariants)
                {
                    if (variant.Sprite == null) continue;
                    roll -= NormalizeWeight(variant.Weight);
                    if (roll <= 0d) return variant.Sprite;
                }
            }

            return GetPreviewSprite();
        }

        private int CountPool()
        {
            int count = _sprite != null ? 1 : 0;

            if (_spriteVariants != null)
            {
                foreach (var variant in _spriteVariants)
                {
                    if (variant.Sprite != null) count++;
                }
            }

            return count;
        }

        private float TotalWeight()
        {
            float total = _sprite != null ? 1f : 0f;

            if (_spriteVariants != null)
            {
                foreach (var variant in _spriteVariants)
                {
                    if (variant.Sprite != null) total += NormalizeWeight(variant.Weight);
                }
            }

            return total;
        }

        private static float NormalizeWeight(float weight)
        {
            return weight <= 0f ? 1f : weight;
        }

        private static System.Random CreateDeterministicRng(Vector2Int position)
        {
            unchecked
            {
                int seed = position.x * 73856093 ^ position.y * 19349663;
                return new System.Random(seed);
            }
        }

        [Serializable]
        public struct SpriteVariant
        {
            public Sprite Sprite;
            [Min(0f)] public float Weight;
        }
    }
}
