using UnityEngine;

namespace Dafral.Grid
{
    public interface IGridEntity
    {
        Vector2Int GridPosition { get; }
        bool IsMoving { get; }
        bool TryMove(Vector2Int direction);
        void SetGridPosition(Vector2Int position);
    }
}