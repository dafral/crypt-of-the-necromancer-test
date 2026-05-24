using Dafral.Game.Map;
using UnityEngine;

namespace Dafral.Enemies
{
    public class Enemy : MonoBehaviour, IEnemy, IGridEntity
    {
        [SerializeField] private EnemyHealth _enemyHealth;
        [SerializeField] private EnemyMovement _enemyMovement;

        public Vector2Int GridPosition => throw new System.NotImplementedException();

        public bool IsMoving => throw new System.NotImplementedException();

        public void SetGridPosition(Vector2Int position)
        {
            throw new System.NotImplementedException();
        }

        public bool TryMove(Vector2Int direction)
        {
            throw new System.NotImplementedException();
        }
    }
}
