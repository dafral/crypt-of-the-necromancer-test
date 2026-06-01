using UnityEngine;

namespace Dafral.Enemies
{
    public static class EnemyFactory
    {
        private static EnemyLibrary _enemyLibrary;

        public static void Initialize(EnemyLibrary enemyLibrary)
        {
            _enemyLibrary = enemyLibrary;
        }

        public static Enemy CreateEnemy(string enemyId, Vector2 position)
        {
            EnemyConfiguration enemyConfiguration = _enemyLibrary.GetEnemyConfigurationById(enemyId);
            var enemy = Object.Instantiate(enemyConfiguration.Prefab, position, Quaternion.identity);
            enemy.Initialize(enemyConfiguration.Data);
            return enemy;
        }
    }
}
