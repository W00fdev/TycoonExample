namespace _Project.Scripts.Infrastructure.States
{
    public interface ITickableState : IState
    {
        public void Tick();
    }
}