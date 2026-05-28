using UnityEngine;

namespace Dafral.Enemies
{
    public class IdleMovementStrategy : IEnemyMovementStrategy
    {
        public Vector2Int? GetNextDirection()
        {
            return null;
        }
    }
}