namespace _Project.Scripts.Infrastructure.States.GameState
{
    public class BootstrapState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;

        public BootstrapState(IStateSwitcher stateSwitcher)
        {
            _stateSwitcher = stateSwitcher;
        }
        
        public void Enter()
        {
        }

        public void Exit()
        {
        }
    }
}