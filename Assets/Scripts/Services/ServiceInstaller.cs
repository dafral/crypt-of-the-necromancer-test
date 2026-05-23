namespace Dafral.Services
{
    public class ServiceInstaller
    {
        public void Install()
        {
            ServiceLocator.Instance.RegisterService<IEventService>(new EventService());
        }
    }
}