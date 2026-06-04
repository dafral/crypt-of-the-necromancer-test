using Dafral.Services;
using UnityEngine;

namespace Dafral.Player
{
    public class PlayerSpawn : MonoBehaviour
    {
        private void Start()
        {
            ServiceLocator.Instance.GetService<IEntityService>().CreatePlayer(transform.position);
        }
    }
}
