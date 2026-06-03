using Dafral.Game.Map;

namespace Dafral.Game.Combat
{
    public interface ICombat
    {
        void Attack(IGridEntity otherEntity);
    }
}
