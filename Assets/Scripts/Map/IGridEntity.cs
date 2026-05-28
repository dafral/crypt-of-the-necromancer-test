using UnityEngine;

namespace Dafral.Game.Map
{
    public interface IGridEntity
    {
        Vector2Int GridPosition { get; }
        GridEntityType EntityType { get; }
        bool TryMove(Vector2Int direction);
        void SetGridPosition(Vector2Int position);
        void Interact(IGridEntity otherEntity);
        void TakeDamage(int damage);
    }
}