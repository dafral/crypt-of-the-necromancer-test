using Dafral.Enemies;
using Dafral.Player;
using UnityEngine;

namespace Dafral.Services
{
    public interface IEntityService
    {
        IPlayer CreatePlayer(Vector2 position);
        IEnemy CreateEnemy(string enemyId, Vector2 position);
    }
}
