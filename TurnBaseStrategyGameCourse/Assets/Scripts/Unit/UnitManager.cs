using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
  public static UnitManager Instance { get; private set; }
  private List<Unit> unitList;
  private List<Unit> friendlyUnitList;
  private List<Unit> enemyUnitList;

  private void Awake()
  {
    if (Instance != null)
    {
      Debug.Log("There's more than one UnitManager" + transform + "-" + Instance);
      Destroy(gameObject);
      return;
    }
    Instance = this;

    unitList = new List<Unit>();
    friendlyUnitList = new List<Unit>();
    enemyUnitList = new List<Unit>();
  }

  private void Start()
  {
    Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
    Unit.OnAnyunitDead += Unit_OnAnyUnitDead;
  }

  private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
  {
    Unit unit = sender as Unit;

    Debug.Log(unit + "spawned");

    unitList.Add(unit);
    if (unit.IsEnemy())
    {
      enemyUnitList.Add(unit);
    }
    else
    {
      friendlyUnitList.Add(unit);
    }
  }

  private void Unit_OnAnyUnitDead(object sender, EventArgs e)
  {
    Unit unit = sender as Unit;

    Debug.Log(unit + "Dead");

    unitList.Remove(unit);
    if (unit.IsEnemy())
    {
      enemyUnitList.Remove(unit);
    }
    else
    {
      friendlyUnitList.Remove(unit);
    }
  }

  public List<Unit> GetUnitList()
  {
    return unitList;
  }

  public List<Unit> FriendlyUnitList()
  {
    return friendlyUnitList;
  }

  public List<Unit> GetEnemyUntList()
  {
    return enemyUnitList;
  }
}
