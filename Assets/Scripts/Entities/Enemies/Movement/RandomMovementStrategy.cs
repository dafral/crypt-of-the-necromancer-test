using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dafral.Enemies
{
    public class RandomMovementStrategy : IEnemyMovementStrategy
    {
        private static readonly Vector2Int[] HorizontalDirections =
        {
            Vector2Int.left, Vector2Int.right
        };

        public Vector2Int? GetNextDirection(Func<Vector2Int, bool> canMove)
        {
            var availableDirections = new List<Vector2Int>(HorizontalDirections.Length);

            foreach (var direction in HorizontalDirections)
            {
                if (canMove(direction))
                {
                    availableDirections.Add(direction);
                }
            }

            if (availableDirections.Count == 0) return null;

            return availableDirections[UnityEngine.Random.Range(0, availableDirections.Count)];
        }
    }
}
