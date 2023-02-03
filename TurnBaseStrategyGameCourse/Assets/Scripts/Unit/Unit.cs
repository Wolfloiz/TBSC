using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
  private GridPosition gridPosition;

  private MoveAction moveAction;
  private SpinAction spinAction;
  private BaseAction[] baseActionArray;
  private int actionPoints = 2;

  private void Awake()
  {
    spinAction = GetComponent<SpinAction>();
    moveAction = GetComponent<MoveAction>();
    baseActionArray = GetComponents<BaseAction>();
  }
  private void Start()
  {
    gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
  }

  private void Update()
  {



    GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    if (newGridPosition != gridPosition)
    {
      LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
      gridPosition = newGridPosition;
    }

  }

  public MoveAction GetMoveAction()
  {
    return moveAction;
  }

  public GridPosition GetGridPosition()
  {
    return gridPosition;
  }

  public SpinAction GetSpinAction()
  {
    return spinAction;
  }

  public BaseAction[] GetBaseActionArray()
  {
    return baseActionArray;
  }

  public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
  {
    if (CanSpendActionsPointsToTakeAction(baseAction))
    {
      SpendActionPoints(baseAction.GetActionPointsCost());
      return true;
    }
    else
      return false;
  }

  public bool CanSpendActionsPointsToTakeAction(BaseAction baseAction)
  {

    if (actionPoints >= baseAction.GetActionPointsCost())
    {
      return true;
    }
    else
      return false;
  }

  private void SpendActionPoints(int amount)
  {
    actionPoints -= amount;
  }

  public int GetActionPoints()
  {
    return actionPoints;
  }


}
