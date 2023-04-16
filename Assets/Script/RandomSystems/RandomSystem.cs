using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSystem : MonoBehaviour
{
   public static float Range(float min, float max)
   {
      return (Random.value * (max - min) + min);
   }


   public static T Roulette<T>(Dictionary<T, float> items)
   {
      float total = 0;

      foreach (var item in items)
      {
         total += item.Value;
      }

      var random = Range(0, total);

      foreach (var item in items)
      {
         if (random < item.Value)
         {
            return item.Key;
         }
         else
         {
            random -= item.Value;
         }
      }

      return default(T);
   }
   
   public static void Shuffle<T>(T[] items)
   {
      for (int i = 0; i < items.Length; i++)
      {
         int random = Random.Range(0, items.Length);
         T randomItem = items[i];
         items[random] = items[i];
         items[i] = randomItem;
      }
   }
   public static void Shuffle<T>(List<T> items)
   {
      for (int i = 0; i < items.Count; i++)
      {
         int random = Random.Range(0, items.Count);
         T randomItem = items[i];
         items[random] = items[i];
         items[i] = randomItem;
      }
   }
}
