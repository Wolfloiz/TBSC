using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
  private const int ACTIONS_POINTS_MAX = 2;

  public static event EventHandler OnAnyActionPointsChanged;

  public static event EventHandler OnAnyUnitSpawned;

  public static event EventHandler OnAnyUnitDead;


  [SerializeField] private bool isEnemy;
  private GridPosition gridPosition;
  private HealthSystem healthSystem;

  private MoveAction moveAction;
  private SpinAction spinAction;

  private ShootAction shootAction;
  private BaseAction[] baseActionArray;
  private int actionPoints = ACTIONS_POINTS_MAX;

  private void Awake()
  {
    spinAction = GetComponent<SpinAction>();
    healthSystem = GetComponent<HealthSystem>();
    moveAction = GetComponent<MoveAction>();
    shootAction = GetComponent<ShootAction>();
    baseActionArray = GetComponents<BaseAction>();
  }
  private void Start()
  {
    gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

    TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;

    healthSystem.OnDead += HealthSystem_OnDead;

    OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
  }

  private void Update()
  {



    GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    if (newGridPosition != gridPosition)
    {
      GridPosition oldGridPosition = gridPosition;
      gridPosition = newGridPosition;

      LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
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

  public ShootAction GetShootAction()
  {
    return shootAction;
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

  public void Damage(int damageAmount)
  {
    healthSystem.Damage(damageAmount);
    Debug.Log(transform + "damaged");

  }

  private void HealthSystem_OnDead(object sender, EventArgs e)
  {
    LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
    Destroy(gameObject);

    OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
  }

  public float GetHealthNormalized()
  {
    return healthSystem.GetHealthNormalized();
  }
}
