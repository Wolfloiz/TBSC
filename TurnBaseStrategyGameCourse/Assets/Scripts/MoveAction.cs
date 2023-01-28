using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
  private Vector3 targetPosition;
  [SerializeField] private Animator unitAnimator;
  [SerializeField] private int maxMoveDistance;

  private Unit unit;
  private void Awake()
  {
    unit = GetComponent<Unit>();
    targetPosition = transform.position;
  }

  private void Update()
  {
    float stoppinDistance = .1f;
    if (Vector3.Distance(transform.position, targetPosition) > stoppinDistance)
    {
      Vector3 moveDirection = (targetPosition - transform.position).normalized;
      float moveSpeed = 4f;
      transform.position += moveDirection * moveSpeed * Time.deltaTime;

      float rotateSpeed = 10f;
      transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

      unitAnimator.SetBool("isWalking", true);
    }
    else
    {
      unitAnimator.SetBool("isWalking", false);
    }
  }

  public void Move(GridPosition targetPosition)
  {
    this.targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);
  }

  public bool IsValidActionGridPosition(GridPosition gridPosition)
  {
    List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
    return validGridPositionList.Contains(gridPosition);
  }

  public List<GridPosition> GetValidActionGridPositionList()
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
}
