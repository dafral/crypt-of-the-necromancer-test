using Dafral.Game.Map;
using UnityEngine;

namespace Dafral.Player
{
    public class PlayerCombat : IPlayerCombat
    {
        public void Attack(IGridEntity otherEntity)
        {
            Debug.Log($"Player attacking {otherEntity.EntityType}");
            otherEntity.TakeDamage(1);
        }
    }
}
