using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
  private const int ACTIONS_POINTS_MAX = 2;

  public static event EventHandler OnAnyActionPointsChanged;

  [SerializeField] private bool isEnemy;
  private GridPosition gridPosition;
  private MoveAction moveAction;
  private SpinAction spinAction;
  private BaseAction[] baseActionArray;
  private int actionPoints = ACTIONS_POINTS_MAX;

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

    TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
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

  public Vector3 GetWorldPosition()
  {
    return transform.position;
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

    OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
  }

  public int GetActionPoints()
  {
    return actionPoints;
  }

  private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
  {
    if ((IsEnemy() && !TurnSystem.Instance.IsPLayerTurn()) ||
    (!IsEnemy() && TurnSystem.Instance.IsPLayerTurn()))
    {
      actionPoints = ACTIONS_POINTS_MAX;

      OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }
  }

  public bool IsEnemy()
  {
    return isEnemy;
  }

  public void Damage()
  {

    Debug.Log(transform + "damaged");

  }
}
