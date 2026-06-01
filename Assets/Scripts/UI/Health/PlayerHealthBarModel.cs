using UnityEngine;

namespace Dafral.Game.UI
{
    public class PlayerHealthBarModel
    {
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }
        public float HealthRatio => MaxHealth == 0 ? 0f : Mathf.Clamp01((float)CurrentHealth / MaxHealth);

        public void SetHealth(int currentHealth, int maxHealth)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
        }
    }
}
