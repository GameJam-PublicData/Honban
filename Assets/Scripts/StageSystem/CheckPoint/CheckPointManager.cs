using System;
using System.Collections.Generic;
using UnityEngine;

namespace StageSystem.CheckPoint
{
public class CheckPointManager  : MonoBehaviour
{
      List<ICheckPoint>  _checkPoints = new();

      public ICheckPoint CheckPoint;

      void Reset()
      {
          _checkPoints.Clear();
          transform.GetChild(0).GetComponentsInChildren(true, _checkPoints);
      }

      void Start()
      {
          _checkPoints.ForEach(checkPoint =>
          {
              
          });
      }
}
}