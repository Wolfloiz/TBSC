using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteractable
{

  [SerializeField] private Material greenMaterial;

  [SerializeField] private Material redMaterial;

  [SerializeField] private MeshRenderer meshRenderer;

  private GridPosition gridPosition;
  private bool isGreen;

  private Action onInteractComplete;

  private bool isActive;

  private float timer;


  private void Start()
  {
    gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
    LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);

    SetColorGreen();
  }

  private void Update()
  {
    if (!isActive)
    {
      return;
    }
    timer -= Time.deltaTime;

    if (timer <= 0f)
    {
      isActive = false;
      onInteractComplete();
    }
  }

  private void SetColorGreen()
  {
    isGreen = true;
    meshRenderer.material = greenMaterial;
    Debug.Log("green");
  }
  private void SetColorRed()
  {
    isGreen = false;
    meshRenderer.material = redMaterial;
    Debug.Log("red");
  }

  public void Interact(Action onInteractionComplete)
  {
    this.onInteractComplete = onInteractionComplete;
    isActive = true;
    timer = .5f;

    if (isGreen)
    {
      Debug.Log("entrei aqui");
      SetColorRed();
    }
    else
    {
      SetColorGreen();
    }
  }
}
