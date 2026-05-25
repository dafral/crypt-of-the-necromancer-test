using System;
using UnityEngine;

namespace Dafral.Enemies
{
    public class EnemyHealth : MonoBehaviour
    {
        private int _health;
        private Action _onDeath;

        public void Initialize(EnemyData enemyData, Action onDeath)
        {
            _health = enemyData.Health;
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

        public void Dispose()
        {
            _health = 0;
            _onDeath = null;
        }
    }
}
