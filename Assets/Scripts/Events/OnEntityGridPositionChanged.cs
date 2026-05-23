using Dafral.Grid;
using UnityEngine;

namespace Dafral.Events
{
    public class OnEntityGridPositionChanged : IEvent
    {
        public IGridEntity Entity { get; }
        public Vector2Int PreviousPosition { get; }
        public Vector2Int NewPosition { get; }

        public OnEntityGridPositionChanged(IGridEntity entity, Vector2Int previousPosition, Vector2Int newPosition)
        {
            Entity = entity;
            PreviousPosition = previousPosition;
            NewPosition = newPosition;
        }
    }
}
