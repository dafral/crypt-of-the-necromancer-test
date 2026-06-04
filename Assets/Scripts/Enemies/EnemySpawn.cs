using Dafral.Events;
using Dafral.Services;
using UnityEngine;

namespace Dafral.Enemies
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] private string _enemyId;
        
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
            EnemyFactory.CreateEnemy(_enemyId, transform.position);
        }
    }
}
