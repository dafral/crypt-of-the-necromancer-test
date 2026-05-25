using Dafral.Game.Map;

namespace Dafral.Enemies
{
    public interface IEnemyMovementStrategy
    {
        bool TryMove();
    }
}
