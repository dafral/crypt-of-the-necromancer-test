using UnityEngine;

namespace Dafral.Player
{
    public class PlayerFactory
    {
        private PlayerConfiguration _playerConfiguration;

        public PlayerFactory(PlayerConfiguration playerConfiguration)
        {
            _playerConfiguration = playerConfiguration;
        }

        public IPlayer CreatePlayer(Vector2 position)
        {
            var player = Object.Instantiate(_playerConfiguration.Prefab, position, Quaternion.identity);
            player.Initialize(_playerConfiguration.Data);
            return player;
        }
    }
}
