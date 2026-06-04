using Dafral.Events;
using Dafral.Game.Combat;
using Dafral.Game.Map;
using UnityEngine;

namespace Dafral.Player
{
    public class Player : GridEntity, IPlayer
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private EntityInteract _entityInteract;
        [SerializeField] private PlayerAnimation _playerAnimation;

        private IPlayerMovement PlayerMovement => _playerMovement;
        private IEntityInteract EntityInteract => _entityInteract;
        private IPlayerAnimation PlayerAnimation => _playerAnimation;

        public override GridEntityType EntityType => GridEntityType.Player;

        public void Initialize(PlayerData playerData)
        {
            RegisterOnGrid();

            EntityInteract.Initialize(GridEntityType.Enemy, new Combat(playerData.Damage));
            PlayerMovement.Initialize(this, this, transform, playerData.Movement);
            PlayerAnimation.Initialize();

            Health.OnHealthChanged += OnHealthChanged;
            Health.OnDied += OnDied;
            Health.Initialize(playerData.Health);
        }

        public override void Interact(IGridEntity otherEntity)
        {
            EntityInteract.Interact(otherEntity);
        }

        public void OnDash(Vector2Int direction)
        {
            PlayerAnimation.OnDash(direction);
        }

        public void OnJumped()
        {
            PlayerAnimation.OnJumped();
        }

        public void OnLanded()
        {
            PlayerAnimation.OnLanded();
        }

        public void OnHealthChanged(int currentHealth, int maxHealth)
        {
            PlayerAnimation.OnHealthChanged(currentHealth, maxHealth);
            _eventService.RaiseEvent(new OnPlayerHealthChanged(currentHealth, maxHealth));
        }

        public void OnDied()
        {
            PlayerAnimation.OnDied();
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