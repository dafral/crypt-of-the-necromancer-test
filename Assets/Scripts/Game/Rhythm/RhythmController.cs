using System;
using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game
{
    public class RhythmController : MonoBehaviour
    {
        private IEventService _eventService;
        private float _tempoBpm;
        private float _elapsedFromLastBeat;
        private int _beatCount;

        public float TempoBpm => _tempoBpm;
        public float BeatInterval => 60f / Mathf.Max(1f, _tempoBpm);
        public float BeatProgress => Mathf.Clamp01(_elapsedFromLastBeat / BeatInterval);
        public float ElapsedFromLastBeat => _elapsedFromLastBeat;
        public int BeatCount => _beatCount;

        public void Initialize(float tempoBpm)
        {
            var serviceLocator = ServiceLocator.Instance;
            _eventService = serviceLocator.GetService<IEventService>();
            serviceLocator.GetService<IUIService>().CreateRhythmBar(this);
            _tempoBpm = Mathf.Max(1f, tempoBpm);
            _elapsedFromLastBeat = 0f;
            _beatCount = 0;
        }

        private void Update()
        {
            _elapsedFromLastBeat += Time.deltaTime;

            float beatInterval = BeatInterval;
            while (_elapsedFromLastBeat >= beatInterval)
            {
                _elapsedFromLastBeat -= beatInterval;
                _beatCount++;
                _eventService.RaiseEvent(new OnBeatTriggered());
            }
        }
    }
}
