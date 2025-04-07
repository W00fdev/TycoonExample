using _Project.Scripts.Infrastructure.States;
using _Project.Scripts.LogicModule;
using UnityEngine;
using UnityEngine.AI;

namespace _Project.Scripts.Components.Enemies
{
    public interface IStateMachineEnemy : IStateSwitcher
    {
        public Animator Animator { get; }
        public NavMeshAgent Agent { get; }
        
        public VolumePivot Target { get; }
    }
}