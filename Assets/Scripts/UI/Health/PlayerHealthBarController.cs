using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.UI
{
    public class PlayerHealthBarController : MonoBehaviour
    {
        [SerializeField] private PlayerHealthBarView _healthBarView;

        private IEventService _eventService;
        private PlayerHealthBarModel _model;

        public void Initialize()
        {
            _model = new PlayerHealthBarModel();
            _eventService = ServiceLocator.Instance.GetService<IEventService>();
            _eventService.Subscribe<OnPlayerHealthChanged>(OnPlayerHealthChanged);
        }

        private void OnDestroy()
        {
            _eventService.Unsubscribe<OnPlayerHealthChanged>(OnPlayerHealthChanged);
        }

        private void OnPlayerHealthChanged(OnPlayerHealthChanged eventData)
        {
            int previousHealth = _model.CurrentHealth;
            _model.SetHealth(eventData.CurrentHealth, eventData.MaxHealth);

            if (eventData.CurrentHealth < previousHealth)
            {
                _healthBarView.TriggerDamagedAnimation();
            }
        }

        private void Update()
        {
            _healthBarView.Render(_model.CurrentHealth, _model.MaxHealth);
        }
    }
}
