using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
  public enum State
  {
    WaitingEnemyTurn,
    TakingTurn,
    Busy,
  }

  private State state;

  private float timer;

  private void Awake()
  {
    state = State.WaitingEnemyTurn;
  }

  private void Start()
  {
    TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
  }

  void Update()
  {
    if (TurnSystem.Instance.IsPLayerTurn())
    {
      return;
    }

    switch (state)
    {
      case State.WaitingEnemyTurn:
        break;

      case State.TakingTurn:
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
          state = State.Busy;
          if (TryTakeEnemyAIAction(SetStateTakingTurn))
            state = State.Busy;
        }
        else
        {
          TurnSystem.Instance.NextTurn();

          // TurnSystem.Instance.NextTurn();
        }
        break;
      case State.Busy:
        break;
    }
  }

  private void SetStateTakingTurn()
  {
    timer = 0.5f;
    state = State.TakingTurn;
  }

  private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
  {
    if (!TurnSystem.Instance.IsPLayerTurn())
    {
      state = State.TakingTurn;
      timer = 2f;
    }
  }

  private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
  {
    Debug.Log("Take Enemy AI Action");

    foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUntList())
    {
      if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
        return true;
    }
    return false;

  }

  private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
  {
    SpinAction spinAction = enemyUnit.GetSpinAction();


    GridPosition actionGridPosition = enemyUnit.GetGridPosition();

    if (!spinAction.IsValidActionGridPosition(actionGridPosition))
    {
      return false;
    }

    if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction))
    {
      return false;
    }

    Debug.Log("spinAction");

    spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
    return true;
  }
}
