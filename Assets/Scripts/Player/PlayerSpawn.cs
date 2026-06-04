using UnityEngine;

namespace Dafral.Player
{
    public class PlayerSpawn : MonoBehaviour
    {
        private void Start()
        {
            PlayerFactory.CreatePlayer(transform.position);
        }
    }
}
