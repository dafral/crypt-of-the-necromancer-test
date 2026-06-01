using Dafral.Events;
using Dafral.Game.Combat;
using Dafral.Game.Map;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Player
{
    public class Player : GridEntity
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private EntityInteract _entityInteract;
        private IEventService _eventService;

        public override GridEntityType EntityType => GridEntityType.Player;

        public void Initialize(PlayerData playerData)
        {
            RegisterOnGrid();
            _eventService = ServiceLocator.Instance.GetService<IEventService>();

            _entityInteract.Initialize(GridEntityType.Enemy, new Combat(playerData.Damage));
            _playerMovement.Initialize(this, transform, playerData.Movement);

            _health.OnHealthChanged += OnHealthChanged;
            _health.OnDied += OnDied;
            _health.Initialize(playerData.Health);
        }

        public override void Interact(IGridEntity otherEntity)
        {
            _entityInteract.Interact(otherEntity);
        }

        private void OnHealthChanged(int currentHealth, int maxHealth)
        {
            _eventService.RaiseEvent(new OnPlayerHealthChanged(currentHealth, maxHealth));
        }

        private void OnDied()
        {
            _eventService.RaiseEvent(new OnPlayerDied());
        }
    }
}
