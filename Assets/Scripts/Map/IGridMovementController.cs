using System;
using UnityEngine;

namespace Dafral.Game.Map
{
    public interface IGridMovementController
    {
        event Action OnJumped;
        event Action OnLanded;

        bool TryToMove(Vector2Int direction);
        bool TryJump(int height);
        bool TryApplyGravityStep();
        bool IsGrounded();
        void Stop();
    }
}
