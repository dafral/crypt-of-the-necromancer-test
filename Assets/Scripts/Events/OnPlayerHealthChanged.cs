namespace Dafral.Events
{
    public class OnPlayerHealthChanged : IEvent
    {
        public int CurrentHealth { get; }
        public int MaxHealth { get; }

        public OnPlayerHealthChanged(int currentHealth, int maxHealth)
        {
            CurrentHealth = currentHealth;
            MaxHealth = maxHealth;
        }
    }
}
