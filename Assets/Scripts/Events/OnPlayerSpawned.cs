using UnityEngine;

namespace Dafral.Events
{
    public class OnPlayerSpawned : IEvent
    {
        public Component Player { get; }

        public OnPlayerSpawned(Component player)
        {
            Player = player;
        }
    }
}
