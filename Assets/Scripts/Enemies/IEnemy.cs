namespace Dafral.Enemies
{
    public interface IEnemy
    {
        void Initialize(EnemyData enemyData);
        void Dispose();
    }
}
