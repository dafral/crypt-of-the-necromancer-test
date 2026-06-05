using Dafral.Events;
using Dafral.Game;
using Dafral.Game.Map;
using UnityEngine;

namespace Dafral.Services
{
    public class GameService : IGameService
    {
        private readonly GameConfiguration _gameConfiguration;
        private readonly IGridService _gridService;
        private readonly IUIService _uiService;
        private readonly IEventService _eventService;
        private int _currentLevelIndex;
        private RhythmController _rhythmController;

        public GameService(
            GameConfiguration gameConfiguration, 
            IGridService gridService, 
            IUIService uiService,
            IEventService eventService)
        {
            _gameConfiguration = gameConfiguration;
            _gridService = gridService;
            _uiService = uiService;
            _eventService = eventService;
            _eventService.Subscribe<OnPlayerDied>(OnPlayerDied);
        }

        public void StartGame()
        {
            CreateGameComponents();
            LoadLevel(_gameConfiguration.Levels[0]);
            _eventService.RaiseEvent(new OnGameStarted());
        }

        public void RestartGame()
        {
            _currentLevelIndex = 0;
            LoadLevel(_gameConfiguration.Levels[_currentLevelIndex]);
        }

        private void CreateGameComponents()
        {
            _uiService.CreatePlayerHealthBar();
            _rhythmController = Object.Instantiate(_gameConfiguration.RhythmController);
        }

        public void RestartCurrentLevel()
        {
            LoadLevel(_gameConfiguration.Levels[_currentLevelIndex]);
        }

        public void BeatLevel()
        {
            _currentLevelIndex++;
            if (_currentLevelIndex >= _gameConfiguration.Levels.Length)
            {
                BeatGame();
                return;
            }

            LoadLevel(_gameConfiguration.Levels[_currentLevelIndex]);
        }

        private void BeatGame()
        {
            _uiService.ShowGameOverScreen(true);
        }

        private void OnPlayerDied(OnPlayerDied eventData)
        {
            _uiService.ShowGameOverScreen(false);
        }

        private void LoadLevel(LevelConfiguration levelConfiguration)
        {
            _gridService.LoadMap(levelConfiguration.MapConfiguration);
            _rhythmController.Initialize(levelConfiguration.LevelData.RhythmTempo);
        }
    }
}