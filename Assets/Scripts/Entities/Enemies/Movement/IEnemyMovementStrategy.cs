using System;
using UnityEngine;

namespace Dafral.Enemies
{
    public interface IEnemyMovementStrategy
    {
        Vector2Int? GetNextDirection(Func<Vector2Int, bool> canMove);
    }
}
