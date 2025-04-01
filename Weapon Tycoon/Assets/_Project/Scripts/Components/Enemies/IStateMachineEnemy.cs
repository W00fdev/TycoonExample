using _Project.Scripts.Components.Character.States;
using _Project.Scripts.Infrastructure.States;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Components.Enemies
{
    public interface IStateMachineEnemy : IStateSwitcher
    {
        public Animator Animator { get; }
        public NavMeshAgent Agent { get; }
        
        public Transform Target { get; }
    }
}