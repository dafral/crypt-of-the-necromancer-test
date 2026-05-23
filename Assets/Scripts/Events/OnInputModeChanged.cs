using Dafral.CustomInput;

namespace Dafral.Events
{
    public class OnInputModeChanged : IEvent
    {
        public readonly InputReader NewInput;

        public OnInputModeChanged(InputReader reader)
        {
            NewInput = reader;
        }
    }
}
