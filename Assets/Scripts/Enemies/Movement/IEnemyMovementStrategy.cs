using UnityEngine;

namespace Dafral.Enemies
{
    public interface IEnemyMovementStrategy
    {
        Vector2Int? GetNextDirection();
    }
}
