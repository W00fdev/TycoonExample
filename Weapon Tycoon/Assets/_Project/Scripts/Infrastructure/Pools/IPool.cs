using _Project.Scripts.LogicModule.Views;

namespace _Project.Scripts.Infrastructure.Factories
{
    public interface IPool
    {
        public PooledView Next();

        public void Warm(int count);
    }

}