using Dafral.Enemies;
using Dafral.Player;
using UnityEngine;

namespace Dafral.Services
{

    public class EntityService : IEntityService
    {
        private PlayerFactory _playerFactory;
        private EnemyFactory _enemyFactory;

        public EntityService(PlayerConfiguration playerConfiguration, EnemyLibrary enemyLibrary)
        {
            _playerFactory = new PlayerFactory(playerConfiguration);
            _enemyFactory = new EnemyFactory(enemyLibrary);
        }

        public IPlayer CreatePlayer(Vector2 position)
        {
            return _playerFactory.CreatePlayer(position);
        }

        public IEnemy CreateEnemy(string enemyId, Vector2 position)
        {
            return _enemyFactory.CreateEnemy(enemyId, position);
        }
    }
}
