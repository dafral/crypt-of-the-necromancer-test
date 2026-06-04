using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Player
{
    public class PlayerSpawn : MonoBehaviour
    {
        private void Start()
        {
            ServiceLocator.Instance.GetService<IEventService>().Subscribe<OnGridLoaded>(OnGridLoaded);
        }

        private void OnDestroy()
        {
            ServiceLocator.Instance.GetService<IEventService>().Unsubscribe<OnGridLoaded>(OnGridLoaded);
        }

        private void OnGridLoaded(OnGridLoaded eventData)
        {
            PlayerFactory.CreatePlayer(transform.position);
        }
    }
}
