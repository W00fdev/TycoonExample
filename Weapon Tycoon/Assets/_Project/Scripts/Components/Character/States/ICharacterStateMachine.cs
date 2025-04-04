using _Project.Scripts.Infrastructure.States;
using UnityEngine;

namespace _Project.Scripts.Components.Character.States
{
    public interface ICharacterStateMachine : IStateSwitcher
    {
        public Animator Animator { get; }
        public CharacterController Controller { get; }
        public bool IsGrounded { get; }
    }
}