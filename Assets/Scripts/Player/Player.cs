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
        public GridEntityType EntityType => GridEntityType.Player;

        public void Initialize(PlayerData playerData)
        {
            var gridService = ServiceLocator.Instance.GetService<IGridService>();
            _gridPosition = gridService.GetGridPosition(transform.position);
            gridService.TryPlaceEntity(this, _gridPosition);

            _playerInteract.Initialize(new PlayerCombat());
            _playerMovement.Initialize(this, transform, playerData.Movement);
        }

        public void SetGridPosition(Vector2Int position)
        {
            _gridPosition = position;
        }

        public bool TryMove(Vector2Int direction)
        {
            return true;
            //return _playerMovement.TryToMove(direction);
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
