using System;

namespace Dafral.Player
{
    [Serializable]
    public struct PlayerMovementData
    {
        public float MoveDuration;
        public float FallStepDuration;
        public int JumpHeight;
        public float EarlyWindowDuration;
        public float LateWindowDuration;
        public float PerfectWindowDuration;
    }
}
