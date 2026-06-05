using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game
{
    [RequireComponent(typeof(AudioSource))]
    public class RhythmController : MonoBehaviour, IRhythmController
    {
        private IEventService _eventService;
        private AudioSource _audioSource;
        private AudioClip _beatSound;
        private float _tempoBpm;
        private float _elapsedFromLastBeat;
        private int _beatCount;

        public float TempoBpm => _tempoBpm;
        public float BeatInterval => 60f / Mathf.Max(1f, _tempoBpm);
        public float BeatProgress => Mathf.Clamp01(_elapsedFromLastBeat / BeatInterval);
        public float ElapsedFromLastBeat => _elapsedFromLastBeat;
        public int BeatCount => _beatCount;

        public void Initialize(float tempoBpm, AudioClip beatSound)
        {
            var serviceLocator = ServiceLocator.Instance;
            _eventService = serviceLocator.GetService<IEventService>();
            serviceLocator.GetService<IUIService>().CreateRhythmBar(this);

            EnsureAudioSource();
            _beatSound = beatSound;

            _tempoBpm = Mathf.Max(1f, tempoBpm);
            _elapsedFromLastBeat = 0f;
            _beatCount = 0;
        }

        private void EnsureAudioSource()
        {
            _audioSource = GetComponent<AudioSource>();
            _audioSource.playOnAwake = false;
        }

        private void Update()
        {
            _elapsedFromLastBeat += Time.deltaTime;

            float beatInterval = BeatInterval;
            while (_elapsedFromLastBeat >= beatInterval)
            {
                _elapsedFromLastBeat -= beatInterval;
                _beatCount++;
                PlayBeatSound();
                _eventService.RaiseEvent(new OnBeatTriggered());
            }
        }

        private void PlayBeatSound()
        {
            _audioSource.PlayOneShot(_beatSound);
        }
    }
}
