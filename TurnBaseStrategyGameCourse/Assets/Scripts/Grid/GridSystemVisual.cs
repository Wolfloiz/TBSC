using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
  [Serializable]
  public struct GridVisualTypeMaterial
  {
    public GridVisualType gridVisualType;
    public Material material;

  }
  public enum GridVisualType
  {
    White,
    Blue,
    Red,
    Yellow,
    RedSoft
  }
  public static GridSystemVisual Instance { get; private set; }

  [SerializeField] private Transform gridSystemVisualSinglePrefab;
  [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;

  private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

  void Awake()
  {
    if (Instance != null)
    {
      Debug.Log("There's more than one LevelGrid!" + transform + "-" + Instance);
      Destroy(gameObject);
      return;
    }
    Instance = this;
  }

  private void Start()
  {
    gridSystemVisualSingleArray = new GridSystemVisualSingle[
        LevelGrid.Instance.GetWidth(),
        LevelGrid.Instance.GetHeight()
        ];
    for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
    {
      for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
      {
        GridPosition gridPosition = new GridPosition(x, z);
        Transform gridSystemSingleTransform = Instantiate(gridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

        gridSystemVisualSingleArray[x, z] = gridSystemSingleTransform.GetComponent<GridSystemVisualSingle>();
      }
    }

    UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
    LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;

    UpdateGridVisual();
  }

  public void HideAllGridPosition()
  {
    for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
    {
      for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
      {
        gridSystemVisualSingleArray[x, z].Hide();
      }
    }
  }

  public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
  {

    foreach (GridPosition gridPosition in gridPositionList)
    {
      gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].
      Show(GetGridVisualTypeMaterial(gridVisualType));
    }
  }

  private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
  {
    List<GridPosition> gridPositionList = new List<GridPosition>();
    for (int x = -range; x <= range; x++)
    {
      for (int z = -range; z <= range; z++)
      {
        GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

        if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
        {
          continue;
        }

        int textDistance = Mathf.Abs(x) + Mathf.Abs(z);

        if (textDistance > range)
          continue;

        gridPositionList.Add(testGridPosition);

      }
    }
    ShowGridPositionList(gridPositionList, gridVisualType);
  }

  private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
  {
    List<GridPosition> gridPositionList = new List<GridPosition>();
    for (int x = -range; x <= range; x++)
    {
      for (int z = -range; z <= range; z++)
      {
        GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

        if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
        {
          continue;
        }

      }
    }
    ShowGridPositionList(gridPositionList, gridVisualType);
  }

  private void UpdateGridVisual()
  {
    HideAllGridPosition();

    Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

    BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

    GridVisualType gridVisualType;

    switch (selectedAction)
    {
      default:
      case MoveAction moveAction:
        gridVisualType = GridVisualType.White;
        break;

      case SpinAction spinAction:
        gridVisualType = GridVisualType.Blue;
        break;

      case ShootAction shootAction:
        gridVisualType = GridVisualType.Red;
        ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootRange(), GridVisualType.RedSoft);
        break;

      case GrenadeAction grenadeAction:
        gridVisualType = GridVisualType.Yellow;
        break;

      case SwordAction swordAction:
        gridVisualType = GridVisualType.Red;
        ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), swordAction.GetMaxSwordDistance(), GridVisualType.RedSoft);
        break;
    }
    ShowGridPositionList(
        selectedAction.GetValidActionGridPositionList(), gridVisualType);
  }

  private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
  {
    UpdateGridVisual();
  }

  private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
  {
    UpdateGridVisual();
  }

  private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
  {
    foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
    {
      if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
      {
        return gridVisualTypeMaterial.material;
      }
    }
    Debug.LogError("Could not find material for grid visual.");
    return null;

  }
}
