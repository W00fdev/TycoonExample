using UnityEngine;

namespace _Project.Scripts.Components.Character
{
    public interface IStateMachine
    {
        public void SwitchState<T>()
            where T : State;
        
        public Animator Animator { get; }
        public CharacterController Controller { get; }
        public bool IsGrounded { get; }
    }
}