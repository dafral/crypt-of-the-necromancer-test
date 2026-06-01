using UnityEngine;

namespace Dafral.Game.Map
{
    [CreateAssetMenu(fileName = "NewSpikeTileConfiguration", menuName = "Dafral/Game/Spike Tile Configuration")]
    public class SpikeTileConfiguration : TileConfiguration, ITileHazard
    {
        [Header("Hazard")]
        [SerializeField] private int _damage;

        public override bool IsWalkable => false;

        public void ApplyHazard(IGridEntity entity)
        {
            entity.TakeDamage(_damage);
        }
    }
}
