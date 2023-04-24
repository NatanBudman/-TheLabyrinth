using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAction : ITreeNode
{

    Action _action;
   public TreeAction(Action action)
   {
        _action = action;

    }

    // Update is called once per frame
    public void Execute()
    {
        if (_action != null)
            _action();

    }

}
