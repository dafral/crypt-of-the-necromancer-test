using UnityEngine;

namespace Dafral.Player
{
    [CreateAssetMenu(fileName = "NewPlayerConfiguration", menuName = "Dafral/Entities/Player Configuration")]
    public class PlayerConfiguration : ScriptableObject
    {
        public Player Prefab;
        public PlayerData Data;
    }
}
