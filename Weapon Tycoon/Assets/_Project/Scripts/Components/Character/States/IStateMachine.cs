using UnityEngine;

namespace _Project.Scripts.Components.Character
{
    /// FIXME: interface segregation apply
    public interface IStateMachine
    {
        public void SwitchState<T>()
            where T : IState;
        
        public Animator Animator { get; }
        public CharacterController Controller { get; }
        public InputReader InputReader { get; }
        public bool IsGrounded { get; }
    }
}