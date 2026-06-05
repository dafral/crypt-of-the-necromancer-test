using Dafral.Services;
using UnityEngine;

namespace Dafral.Enemies
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] private EnemyConfiguration _enemyConfiguration;

        private void Start()
        {
            ServiceLocator.Instance.GetService<IEntityService>().CreateEnemy(_enemyConfiguration.Id, transform.position);
        }
    }
}
