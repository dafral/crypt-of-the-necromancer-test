using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Game.Map
{
    public class EndTile : MonoBehaviour
    {
        private IEventService _eventService;
        private IGridService _gridService;
        private IGameService _gameService;
        private Vector2Int _gridPosition;

        private void OnEnable()
        {
            _eventService = ServiceLocator.Instance.GetService<IEventService>();
            _gridService = ServiceLocator.Instance.GetService<IGridService>();
            _gameService = ServiceLocator.Instance.GetService<IGameService>();
            _gridPosition = _gridService.GetGridPosition(transform.position);

            _eventService.Subscribe<OnEntityGridPositionChanged>(OnEntityGridPositionChanged);
        }

        private void OnDisable()
        {
            _eventService.Unsubscribe<OnEntityGridPositionChanged>(OnEntityGridPositionChanged);
        }

        private void OnEntityGridPositionChanged(OnEntityGridPositionChanged e)
        {
            if (e.NewPosition != _gridPosition || e.Entity.EntityType != GridEntityType.Player) return;

            _gameService.BeatLevel();
        }
    }
}
