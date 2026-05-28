using Dafral.Game.Map;

namespace Dafral.Player
{
    public interface IPlayerCombat
    {
        void Attack(IGridEntity otherEntity);
    }
}
