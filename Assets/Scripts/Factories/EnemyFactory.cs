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

        public static IEnemy CreateEnemy(string enemyId,Vector2 position)
        {
            EnemyConfiguration enemyConfiguration = _enemyLibrary.GetEnemyConfigurationById(enemyId);
            return Object.Instantiate(enemyConfiguration.Prefab, position, Quaternion.identity);
        }
    }
}
