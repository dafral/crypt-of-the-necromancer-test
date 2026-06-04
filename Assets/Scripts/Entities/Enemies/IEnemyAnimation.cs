namespace Dafral.Enemies
{
    public interface IEnemyAnimation
    {
        void Initialize();
        void OnHealthChanged(int currentHealth, int maxHealth);
        void OnDied();
    }
}
