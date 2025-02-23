using System;

namespace _Project.Scripts.Components.Character
{
    [Serializable]
    public abstract class State
    {
        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}