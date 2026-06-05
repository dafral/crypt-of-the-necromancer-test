using UnityEngine;

namespace Dafral.Enemies
{
    [CreateAssetMenu(fileName = "NewEnemyConfiguration", menuName = "Dafral/Entities/Enemy Configuration")]
    public class EnemyConfiguration : ScriptableObject
    {
        public string Id;
        public Enemy Prefab;
        public EnemyData Data;
    }
}
