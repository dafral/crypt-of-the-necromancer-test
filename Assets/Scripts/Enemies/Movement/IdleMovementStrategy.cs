using Dafral.Game.Map;

namespace Dafral.Enemies
{
    public class IdleMovementStrategy : IEnemyMovementStrategy
    {
        public bool TryMove()
        {
            return false;
        }
    }
}