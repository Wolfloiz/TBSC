using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeAction : BaseAction
{
  private int maxThrowDistance = 7;

  [SerializeField] private Transform grenaProjectilePrefab;
  private void Update()
  {
    if (!isActive)
    {
      return;
    }

    ActionComplete();
  }
  public override string GetActionName()
  {
    return "Grenade";
  }

  public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
  {
    return new EnemyAIAction
    {
      gridPosition = gridPosition,
      actionValue = 0,
    };
  }

  public override List<GridPosition> GetValidActionGridPositionList()
  {
    GridPosition unitGridPosition = unit.GetGridPosition();
    return GetValidActionGridPositionList(unitGridPosition);
  }

  public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
  {
    List<GridPosition> validGridPositionList = new List<GridPosition>();

    for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
    {
      for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
      {
        GridPosition offsetGridPosition = new GridPosition(x, z);
        GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

        if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
        {
          continue;
        }

        int textDistance = Mathf.Abs(x) + Mathf.Abs(z);

        if (textDistance > maxThrowDistance)
          continue;

        validGridPositionList.Add(testGridPosition);

      }
    }

    return validGridPositionList;
  }

  public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
  {
    Transform grenadeProjectileTransform = Instantiate(grenaProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
    GrenadeProjectile grenadeProjectile = grenadeProjectileTransform.GetComponent<GrenadeProjectile>();
    grenadeProjectile.Setup(gridPosition, onGrenadeBehaviorComplete);
    ActionStart(onActionComplete);
  }

  private void onGrenadeBehaviorComplete()
  {
    ActionComplete();
  }
}
