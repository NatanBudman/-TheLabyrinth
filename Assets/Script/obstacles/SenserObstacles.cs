using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SenserObstacles 
{
   private Transform _origin;
   private LayerMask _ObstacleLayers;
   private float _radius;
   private float _angle;
   private Collider[] Obstacle;
   
   
   public SenserObstacles(Transform origin, LayerMask ObstacleLayers,
      int ObstacleLenght, float radius, float angle)
   {
      _origin = origin;
      _ObstacleLayers = ObstacleLayers;
      _radius = radius;
      _angle = angle;
      Obstacle = new Collider[ObstacleLenght];
   }

   public void ObstacleDetected()
   {
      int countObstacle = Physics.OverlapSphereNonAlloc(_origin.position, _radius, Obstacle, _ObstacleLayers);

      for (int i = 0; i < countObstacle; i++)
      {
         Collider col = Obstacle[i];
         
         Vector3 diffPoint = col.transform.position - _origin.position;

         float angleToPoint = Vector3.Angle(_origin.forward, diffPoint);
         
         if (angleToPoint > _angle / 2) continue;
         
         IObstacles obstacles = null;
         
         if (col.GetComponent<IObstacles>() != null) obstacles = col.GetComponent<IObstacles>();
         else continue;
         
         obstacles.Execute();   
         

      }

   }

}
