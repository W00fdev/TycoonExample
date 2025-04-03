namespace _Project.Scripts.Infrastructure.States
{
    public interface IStateSwitcher
    {
        public void SwitchState<T>()
            where T : IState;
    }
}