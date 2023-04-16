using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flee : Seek
{
   public Flee(Transform origin, Transform _target) : base(origin, _target)
   {
      
   }

   public override Vector3 GetDir()
   {
      return -base.GetDir();
   }
}
