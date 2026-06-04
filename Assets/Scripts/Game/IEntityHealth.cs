using System;

namespace Dafral.Game
{
    public interface IEntityHealth
    {
        int CurrentHealth { get; }
        int MaxHealth { get; }
        float Ratio { get; }
        event Action<int, int> OnHealthChanged;
        event Action OnDied;
        void Initialize(int maxHealth);
    }
}
