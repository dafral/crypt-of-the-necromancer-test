using UnityEngine;

namespace Dafral.Player
{
    public static class PlayerFactory
    {
        private static PlayerConfiguration _playerConfiguration;

        public static void Initialize(PlayerConfiguration playerConfiguration)
        {
            _playerConfiguration = playerConfiguration;
        }

        public static Player CreatePlayer(Vector2 position)
        {
            return Object.Instantiate(_playerConfiguration.Prefab, position, Quaternion.identity);
        }
    }
}
