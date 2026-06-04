using Dafral.Game.Map;

namespace Dafral.Enemies
{
    public interface IEnemyMovement
    {
        void Initialize(IGridEntity gridEntity, EnemyData data);
        void Dispose();
    }
}
