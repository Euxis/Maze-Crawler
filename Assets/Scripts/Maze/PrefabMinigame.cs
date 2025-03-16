using System;
using System.Collections.Generic;
using UnityEngine;

public class PrefabMinigame : MonoBehaviour
{
   public static readonly HashSet<PrefabMinigame> Nodes = new HashSet<PrefabMinigame>();
   void Awake()
   {
      Nodes.Add(this);
   }

   private void OnDestroy()
   {
      Nodes.Remove(this);
   }
}
