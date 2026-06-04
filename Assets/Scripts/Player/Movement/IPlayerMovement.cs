using Dafral.Game.Map;
using UnityEngine;

namespace Dafral.Player
{
    public interface IPlayerMovement
    {
        void Initialize(IGridEntity gridEntity, IPlayer player, Transform playerTransform, PlayerMovementData movementData);
        void Dispose();
    }
}
