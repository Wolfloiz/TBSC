using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdater : MonoBehaviour
{

  private void Start()
  {
    DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnydestroyed;
  }

  private void DestructibleCrate_OnAnydestroyed(object sender, EventArgs e)
  {
    DestructibleCrate destructibleCrate = sender as DestructibleCrate;
    Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GetGridPosition(), true);
  }
}
