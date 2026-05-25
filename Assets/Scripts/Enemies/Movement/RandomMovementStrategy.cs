using Dafral.Game.Map;
using UnityEngine;

namespace Dafral.Enemies
{
    public class RandomMovementStrategy : IEnemyMovementStrategy
    {
        private readonly IGridEntity _entity;

        public RandomMovementStrategy(IGridEntity entity)
        {
            _entity = entity;
        }

        private static readonly Vector2Int[] CardinalDirections =
        {
            Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right
        };

        public bool TryMove()
        {
            var direction = CardinalDirections[Random.Range(0, CardinalDirections.Length)];
            return _entity.TryMove(direction);
        }
    }
}
