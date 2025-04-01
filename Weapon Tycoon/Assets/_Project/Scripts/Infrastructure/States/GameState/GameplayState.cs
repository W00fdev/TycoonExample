namespace _Project.Scripts.Infrastructure.States.GameState
{
    public class GameplayState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;

        public GameplayState(IStateSwitcher stateSwitcher)
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