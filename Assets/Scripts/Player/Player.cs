using Dafral.Game.Map;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Player
{
    public class Player : MonoBehaviour, IPlayer, IGridEntity
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerInteract _playerInteract;
        private Vector2Int _gridPosition;

        public Vector2Int GridPosition => _gridPosition;
        public bool IsMoving => _playerMovement.IsMoving;

        public GridEntityType EntityType => GridEntityType.Player;

        private void Start()
        {
            var gridService = ServiceLocator.Instance.GetService<IGridService>();
            _gridPosition = gridService.GetGridPosition(transform.position);
            gridService.TryPlaceEntity(this, _gridPosition);
            _playerInteract.Initialize(new PlayerCombat());
            _playerMovement.Initialize(this, transform);
        }

        public void SetGridPosition(Vector2Int position)
        {
            _gridPosition = position;
        }

        public bool TryMove(Vector2Int direction)
        {
            return _playerMovement.TryToMove(direction);
        }

        public void Interact(IGridEntity otherEntity)
        {
            _playerInteract.Interact(otherEntity);
        }

        public void TakeDamage(int damage)
        {
        }
    }
}
