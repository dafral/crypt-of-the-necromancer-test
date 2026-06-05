using System;
using UnityEngine;

namespace Dafral.Enemies
{
    public class IdleMovementStrategy : IEnemyMovementStrategy
    {
        public Vector2Int? GetNextDirection(Func<Vector2Int, bool> canMove)
        {
            return null;
        }
    }
}