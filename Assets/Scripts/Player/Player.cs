using Dafral.Grid;
using UnityEngine;

namespace Dafral.Player
{
    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private PlayerMovement _playerMovement;

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
