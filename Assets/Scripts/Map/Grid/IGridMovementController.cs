using System;
using UnityEngine;

namespace Dafral.Game.Map
{
    public enum GridMoveResult
    {
        Blocked,
        Moved,
        Interacted
    }

    public interface IGridMovementController
    {
        event Action OnJumped;
        event Action OnLanded;

        GridMoveResult TryToMove(Vector2Int direction);
        bool TryJump(int height);
        bool TryApplyGravityStep();
        bool IsGrounded();
        void Stop();
    }
}
