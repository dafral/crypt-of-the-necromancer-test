using System;
using UnityEngine;

namespace Dafral.Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        private int _health = 1;
        private Action _onDeath;

        public void Initialize(Action onDeath)
        {
            _onDeath = onDeath;
        }

        public void TakeDamage(int damage)
        {
            _health -= damage;
            if (_health <= 0)
            {
                _onDeath?.Invoke();
            }
        }
    }
}
