using Dafral.Game.Map;
using UnityEngine;

namespace Dafral.Game.Combat
{
    public class EntityInteract : MonoBehaviour, IEntityInteract
    {
        private GridEntityType _targetType;
        private ICombat _combat;

        public void Initialize(GridEntityType targetType, ICombat combat)
        {
            _targetType = targetType;
            _combat = combat;
        }

        public void Interact(IGridEntity otherEntity)
        {
            if (otherEntity.EntityType == _targetType)
            {
                _combat.Attack(otherEntity);
            }
        }

        public void Dispose()
        {
            _combat = null;
            _targetType = GridEntityType.None;
        }

    }
}
