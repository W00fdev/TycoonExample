using UnityEngine;

namespace _Project.Scripts.Components.Character.States
{
    public interface IStateMachineCharacter : IStateSwitcher
    {
        public Animator Animator { get; }
        public CharacterController Controller { get; }
        public InputReader InputReader { get; }
        public bool IsGrounded { get; }
    }
}