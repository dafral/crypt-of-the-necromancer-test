using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Player
{
    public class PlayerSpawn : MonoBehaviour
    {
        private IEventService _eventService;

        private void Start()
        {
            _eventService = ServiceLocator.Instance.GetService<IEventService>();
            _eventService.Subscribe<OnGridLoaded>(OnGridLoaded);
            SpawnPlayer();
        }

        private void OnDestroy()
        {
            _eventService?.Unsubscribe<OnGridLoaded>(OnGridLoaded);
        }

        private void OnGridLoaded(OnGridLoaded eventData)
        {
            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            PlayerFactory.CreatePlayer(transform.position);
        }
    }
}
