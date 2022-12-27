using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedUnit : MonoBehaviour
{
  [SerializeField] private Unit unit;

  private MeshRenderer meshRenderer;

  private void Awake()
  {
    meshRenderer = GetComponent<MeshRenderer>();
  }

}
