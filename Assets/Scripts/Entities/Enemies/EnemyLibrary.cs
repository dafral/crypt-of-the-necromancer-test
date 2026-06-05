using System.Linq;
using UnityEngine;

namespace Dafral.Enemies
{
    [CreateAssetMenu(fileName = "NewEnemyLibrary", menuName = "Dafral/Entities/Enemy Library")]
    public class EnemyLibrary : ScriptableObject
    {
        public EnemyConfiguration[] Enemies;

        public EnemyConfiguration GetEnemyConfigurationById(string enemyId)
        {
            return Enemies.FirstOrDefault(enemy => enemy.Id == enemyId);
        }
    }
}
