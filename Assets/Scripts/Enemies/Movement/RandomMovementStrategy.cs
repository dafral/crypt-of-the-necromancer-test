using UnityEngine;

namespace Dafral.Enemies
{
    public class RandomMovementStrategy : IEnemyMovementStrategy
    {
        private static readonly Vector2Int[] HorizontalDirections =
        {
            Vector2Int.left, Vector2Int.right
        };

        public Vector2Int? GetNextDirection()
        {
            return HorizontalDirections[Random.Range(0, HorizontalDirections.Length)];
        }
    }
}
