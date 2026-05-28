using Dafral.Game.Map;
using UnityEngine;

namespace Dafral.Player
{
    public class PlayerInteract : MonoBehaviour
    {
        private IPlayerCombat _playerCombat;

        public void Initialize(IPlayerCombat playerCombat)
        {
            _playerCombat = playerCombat;
        }

        public void Interact(IGridEntity otherEntity)
        {
            switch (otherEntity.EntityType)
            {
                case GridEntityType.Enemy:
                    _playerCombat.Attack(otherEntity);
                    break;
                default:
                    break;
            }
        }
    }
}
