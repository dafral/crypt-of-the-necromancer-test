using Dafral.CustomInput;

namespace Dafral.Services
{
    public interface IInputService
    {
        public InputReader CurrentInput { get; }
        public InputReader ChangeInputMode(InputMode newInput);
    }
}