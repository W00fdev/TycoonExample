namespace _Project.Scripts.Components.Character.States
{
    public interface IStateSwitcher
    {
        public void SwitchState<T>()
            where T : IState;
    }
}