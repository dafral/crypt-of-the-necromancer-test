using Dafral.CustomInput;

namespace Dafral.Game.UI
{
    public class ScreenControlsModel
    {
        private readonly GameplayInputReader _inputReader;

        public GameplayInputReader InputReader => _inputReader;

        public ScreenControlsModel(GameplayInputReader inputReader)
        {
            _inputReader = inputReader;
        }

        public void TriggerMoveLeft()
        {
            _inputReader?.InvokeAction(GameplayInputActions.MoveLeft);
        }

        public void TriggerMoveRight()
        {
            _inputReader?.InvokeAction(GameplayInputActions.MoveRight);
        }

        public void TriggerJump()
        {
            _inputReader?.InvokeAction(GameplayInputActions.Jump);
        }
    }
}
