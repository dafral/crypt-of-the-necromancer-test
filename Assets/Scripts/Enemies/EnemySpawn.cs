using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Enemies
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] private string _enemyId;

        private IEventService _eventService;

        private void Start()
        {
            _eventService = ServiceLocator.Instance.GetService<IEventService>();
            _eventService.Subscribe<OnGridLoaded>(OnGridLoaded);
            SpawnEnemy();
        }

        private void OnDestroy()
        {
            _eventService?.Unsubscribe<OnGridLoaded>(OnGridLoaded);
        }

        private void OnGridLoaded(OnGridLoaded eventData)
        {
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            EnemyFactory.CreateEnemy(_enemyId, transform.position);
        }
    }
}
