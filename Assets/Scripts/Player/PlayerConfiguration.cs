using UnityEngine;

namespace Dafral.Player
{
    [CreateAssetMenu(fileName = "PlayerConfiguration", menuName = "Dafral/Configurations/Player")]
    public class PlayerConfiguration : ScriptableObject
    {
        public Player Prefab;
        public PlayerData Data;
    }
}
