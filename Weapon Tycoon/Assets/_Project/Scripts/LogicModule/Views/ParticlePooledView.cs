namespace _Project.Scripts.LogicModule.Views
{
    public class ParticlePooledView : PooledView
    {
        void OnParticleSystemStopped() => ReturnToPool();
    }
}