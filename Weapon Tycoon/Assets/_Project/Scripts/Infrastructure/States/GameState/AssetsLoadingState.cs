namespace _Project.Scripts.Infrastructure.States.GameState
{
    public class AssetsLoadingState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;

        public AssetsLoadingState(IStateSwitcher stateSwitcher)
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