using _Project.Scripts.Infrastructure.Pools;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;

namespace _Project.Scripts.Infrastructure.States.GameState
{
    [UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)] 
    public class AssetsLoadingState : IState
    {
        private readonly IStateSwitcher _stateSwitcher;
        private readonly ObjectPoolService _poolService;
        
        public AssetsLoadingState(GameStateMachine stateSwitcher, ObjectPoolService poolService)
        {
            _stateSwitcher = stateSwitcher;
            _poolService = poolService;
        }
        
        public void Enter()
        {
            LoadAssets().Forget();
        }

        private async UniTaskVoid LoadAssets()
        {
            await _poolService.LoadAssets();
            
            _poolService.Warm();
            _stateSwitcher.SwitchState<GameplayState>();
        }

        public void Exit()
        {
        }
    }
}