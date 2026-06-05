using UnityEngine;

namespace Dafral.Game
{
    public interface IRhythmController
    {
        float TempoBpm { get; }
        float BeatInterval { get; }
        float BeatProgress { get; }
        float ElapsedFromLastBeat { get; }
        int BeatCount { get; }

        void Initialize(float tempoBpm, AudioClip beatSound);
    }
}
