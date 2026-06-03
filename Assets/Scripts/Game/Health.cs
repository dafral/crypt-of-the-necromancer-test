using System;
using UnityEngine;

namespace Dafral.Game
{
    public class Health : MonoBehaviour
    {
        private int _currentHealth;
        private int _maxHealth;

        public int CurrentHealth => _currentHealth;
        public int MaxHealth => _maxHealth;
        public float Ratio => _maxHealth == 0 ? 0f : (float)_currentHealth / _maxHealth;

        public event Action<int, int> OnHealthChanged;
        public event Action OnDied;

        public void Initialize(int maxHealth)
        {
            _maxHealth = maxHealth;
            _currentHealth = maxHealth;
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        }

        public void TakeDamage(int damage)
        {
            _currentHealth -= damage;
            OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

            if (_currentHealth <= 0)
            {
                OnDied?.Invoke();
            }
        }
    }
}
