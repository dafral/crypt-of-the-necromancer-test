using UnityEngine;

namespace Dafral.Enemies
{
    public class EnemySpawn : MonoBehaviour
    {
        [SerializeField] private string _enemyId;
        
        private void Start()
        {
            EnemyFactory.CreateEnemy(_enemyId, transform.position);
        }
    }
}
