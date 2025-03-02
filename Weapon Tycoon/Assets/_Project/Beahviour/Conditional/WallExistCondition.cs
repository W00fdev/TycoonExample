using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "WallExist", story: "[Wall] [exist]", category: "Conditions", id: "ecc3c9bb81c2bbbec67294bdc968e661")]
public partial class WallExistCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Wall;
    [SerializeReference] public BlackboardVariable<bool> Exist;

    public override bool IsTrue()
    {
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
