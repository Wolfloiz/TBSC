using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : BaseAction
{
  public event EventHandler OnStartMoving;
  public event EventHandler OnStopMoving;
  private Vector3 targetPosition;
  [SerializeField] private int maxMoveDistance;
  protected override void Awake()
  {
    base.Awake();
    targetPosition = transform.position;
  }

  private void Update()
  {

    if (!isActive)
    {
      return;
    }
    Vector3 moveDirection = (targetPosition - transform.position).normalized;

    float stoppinDistance = .1f;
    if (Vector3.Distance(transform.position, targetPosition) > stoppinDistance)
    {

      float moveSpeed = 4f;
      transform.position += moveDirection * moveSpeed * Time.deltaTime;

      float rotateSpeed = 10f;
      transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

    }
    else
    {

      OnStopMoving?.Invoke(this, EventArgs.Empty);
      ActionComplete();
    }
  }

  public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
  {
    ActionStart(onActionComplete);
    this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
    OnStartMoving?.Invoke(this, EventArgs.Empty);
  }



  public override List<GridPosition> GetValidActionGridPositionList()
  {
    List<GridPosition> validGridPositionList = new List<GridPosition>();

    GridPosition unitGridPosition = unit.GetGridPosition()
    ;

    for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
    {
      for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
      {
        GridPosition offsetGridPosition = new GridPosition(x, z);
        GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

        if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
        {
          continue;
        }
        if (unitGridPosition == testGridPosition)
        {
          // same gridPosition where the unit is already at
          continue;
        }

        if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition))
        {
          continue;
        }

        validGridPositionList.Add(testGridPosition);

      }
    }

    return validGridPositionList;
  }

  public override string GetActionName()
  {
    return "Move";
  }
}
