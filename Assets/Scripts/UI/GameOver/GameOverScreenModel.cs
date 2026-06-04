namespace Dafral.Game.UI
{
    public class GameOverScreenModel
    {
        public bool IsVictory { get; private set; }

        public GameOverScreenModel(bool isVictory)
        {
            IsVictory = isVictory;
        }
    }
}
