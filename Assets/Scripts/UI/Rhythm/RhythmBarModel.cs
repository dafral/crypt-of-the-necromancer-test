using System;
using UnityEngine;

namespace Dafral.Game.UI
{
    public class RhythmBarModel
    {
        public event Action OnBeatTriggered;

        public float BeatProgress { get; private set; }

        public void SetBeatProgress(float beatProgress)
        {
            BeatProgress = Mathf.Clamp01(beatProgress);
        }

        public void NotifyBeat()
        {
            OnBeatTriggered?.Invoke();
        }
    }
}
