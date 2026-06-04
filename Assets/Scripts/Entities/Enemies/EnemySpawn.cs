using Dafral.Services;
using UnityEngine;

namespace Dafral.Enemies
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] private string _enemyId;
        
        private void Start()
        {
            ServiceLocator.Instance.GetService<IEntityService>().CreateEnemy(_enemyId, transform.position);
        }
    }
}
