using Dafral.Game.Map;

namespace Dafral.Events
{
    public class OnGridLoaded : IEvent
    {
        public GridData Grid { get; }
        public GridCoordinateConverter CoordinateConverter { get; }

        public OnGridLoaded(GridData grid, GridCoordinateConverter coordinateConverter)
        {
            Grid = grid;
            CoordinateConverter = coordinateConverter;
        }
    }
}
