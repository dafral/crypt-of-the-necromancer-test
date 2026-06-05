using UnityEngine;

namespace Dafral.Enemies
{
    public class EnemyFactory
    {
        private EnemyLibrary _enemyLibrary;

        public EnemyFactory(EnemyLibrary enemyLibrary)
        {
            _enemyLibrary = enemyLibrary;
        }

        public IEnemy CreateEnemy(string enemyId, Vector2 position)
        {
            EnemyConfiguration enemyConfiguration = _enemyLibrary.GetEnemyConfigurationById(enemyId);
            Enemy enemy = Object.Instantiate(enemyConfiguration.Prefab, position, Quaternion.identity);
            enemy.Initialize(enemyConfiguration.Data);
            return enemy;
        }
    }
}
