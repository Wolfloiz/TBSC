using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
  public static TurnSystem Instance { get; private set; }

  public event EventHandler OnTurnChanged;

  private bool isPlayerTurn = true;
  private int turnNumber = 1;

  private void Awake()
  {
    if (Instance != null)
    {
      Debug.Log("There's more than one TurnSystem!" + transform + "-" + Instance);
      Destroy(gameObject);
      return;
    }
    Instance = this;

  }


  public void NextTurn()
  {
    turnNumber++;

    isPlayerTurn = !isPlayerTurn;

    OnTurnChanged?.Invoke(this, EventArgs.Empty);
  }

  public int GetTurnNumber()
  {
    return turnNumber;
  }

  public bool IsPLayerTurn()
  {
    return isPlayerTurn;
  }
}