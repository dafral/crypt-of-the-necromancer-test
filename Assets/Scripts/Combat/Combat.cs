using Dafral.Game.Map;
using UnityEngine;

namespace Dafral.Game.Combat
{
    public class Combat : ICombat
    {
        private readonly int _damage;

        public Combat(int damage)
        {
            _damage = damage;
        }

        public void Attack(IGridEntity otherEntity)
        {
            Debug.Log($"Attacking {otherEntity.EntityType} for {_damage} damage");
            otherEntity.TakeDamage(_damage);
        }
    }
}
