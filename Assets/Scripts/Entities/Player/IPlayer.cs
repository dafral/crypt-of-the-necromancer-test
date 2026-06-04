using UnityEngine;

namespace Dafral.Player
{
    public interface IPlayer
    {
        void OnDash(Vector2Int direction);
        void OnJumped();
        void OnLanded();
        void OnHealthChanged(int currentHealth, int maxHealth);
        void OnDied();
    }
}
