using Dafral.Game.Map;

namespace Dafral.Game.Combat
{
    public interface IEntityInteract
    {
        void Initialize(GridEntityType targetType, ICombat combat);
        void Interact(IGridEntity otherEntity);
        void Dispose();
    }
}
