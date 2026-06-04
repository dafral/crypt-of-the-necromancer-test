using Dafral.Events;
using Dafral.Game.Combat;
using Dafral.Game.Map;
using UnityEngine;

namespace Dafral.Player
{
    public class Player : GridEntity
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private EntityInteract _entityInteract;

        private IPlayerMovement PlayerMovement => _playerMovement;
        private IEntityInteract EntityInteract => _entityInteract;

        public override GridEntityType EntityType => GridEntityType.Player;

        public void Initialize(PlayerData playerData)
        {
            RegisterOnGrid();

            EntityInteract.Initialize(GridEntityType.Enemy, new Combat(playerData.Damage));
            PlayerMovement.Initialize(this, transform, playerData.Movement);
            
            Health.OnHealthChanged += OnHealthChanged;
            Health.OnDied += OnDied;
            Health.Initialize(playerData.Health);
        }

        public override void Interact(IGridEntity otherEntity)
        {
            EntityInteract.Interact(otherEntity);
        }

        private void OnHealthChanged(int currentHealth, int maxHealth)
        {
            _eventService.RaiseEvent(new OnPlayerHealthChanged(currentHealth, maxHealth));
        }

        private void OnDied()
        {
            _eventService.RaiseEvent(new OnPlayerDied());
            Despawn();
        }

        protected override void Despawn()
        {
            PlayerMovement.Dispose();
            EntityInteract.Dispose();
            Health.OnHealthChanged -= OnHealthChanged;
            Health.OnDied -= OnDied;
            base.Despawn();
        }
    }
}