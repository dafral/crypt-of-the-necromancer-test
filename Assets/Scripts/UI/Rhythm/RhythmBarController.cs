using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.UI
{
    public class RhythmBarController : MonoBehaviour
    {
        [SerializeField] private RhythmBarView _rhythmBarView;

        private IEventService _eventService;
        private RhythmController _rhythmController;
        private RhythmBarModel _model;

        public void Initialize(RhythmController rhythmController)
        {
            _rhythmController = rhythmController;
            CreateModel();
            SubscribeToEvents();
        }

        private void CreateModel()
        {
            _model = new RhythmBarModel();
            _model.OnBeatTriggered += HandleModelBeatTriggered;
        }

        private void SubscribeToEvents()
        {
            _eventService = ServiceLocator.Instance.GetService<IEventService>();
            _eventService.Subscribe<OnBeatTriggered>(OnBeatTriggered);
        }

        private void OnDestroy()
        {
            _eventService.Unsubscribe<OnBeatTriggered>(OnBeatTriggered);
            _model.OnBeatTriggered -= HandleModelBeatTriggered;
        }

        private void OnBeatTriggered(OnBeatTriggered eventData)
        {
            _model?.NotifyBeat();
        }

        private void HandleModelBeatTriggered()
        {
            _rhythmBarView?.TriggerTempoAnimation();
        }

        private void Update()
        {
            _model.SetBeatProgress(_rhythmController.BeatProgress);
            _rhythmBarView.Render(_model.BeatProgress);
        }
    }
}
