using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CiivilChaseState<T> : CivilStateBase<T>
{
    public override void Awake()
    {
        base.Awake();

    }
    public override void Execute()
    {
        base.Execute();
        Debug.Log("CivilSigue");
        _model.Move(_manager.FlockingDir());
        
    }
   
}
