using _Project.Scripts.Components;
using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackWall", story: "[Attack] [Wall]", category: "Action", id: "01f974c62e8ebc6d9adfb97e38930f27")]
public partial class AttackWallAction : Action
{
    [SerializeReference] public BlackboardVariable<Enemy> Attack;
    [SerializeReference] public BlackboardVariable<GameObject> Wall;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

