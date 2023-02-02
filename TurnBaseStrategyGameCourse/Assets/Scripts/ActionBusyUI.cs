using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
  private void Start()
  {
    Hide();
    UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
  }

  // Update is called once per frame

  private void Show()
  {
    gameObject.SetActive(true);
  }

  private void Hide()
  {
    gameObject.SetActive(false);
  }

  private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
  {
    if (isBusy)
      Show();
    if (!isBusy)
      Hide();

  }
}
