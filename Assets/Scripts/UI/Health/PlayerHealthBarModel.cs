namespace Dafral.Game.UI
{
    public class PlayerHealthBarModel
    {
        public int CurrentHealth { get; private set; }
        public int MaxHealth { get; private set; }

        public void SetHealth(int currentHealth, int maxHealth)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
        }
    }
}
