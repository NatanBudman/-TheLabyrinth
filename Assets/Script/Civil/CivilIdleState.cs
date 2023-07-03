using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CivilIdleState<T> : CivilStateBase<T>
{
    public override void Awake()
    {
        base.Awake();

    }
    public override void Execute()
    {
        base.Execute();
        Debug.Log("CivilQuieto");
        _model._rb.velocity = Vector3.zero;


    }
    public override void Sleep()
    {
        base.Sleep();

    }
}
