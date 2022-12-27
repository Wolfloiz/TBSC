using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
  [SerializeField] private Animator unitAnimator;

  private Vector3 targetPosition;

  private void Update()
  {
    unitAnimator.SetBool("isWalking", false);
    float stoppinDistance = .1f;
    if (Vector3.Distance(transform.position, targetPosition) > stoppinDistance)
    {
      Vector3 moveDirection = (targetPosition - transform.position).normalized;
      float moveSpeed = 4f;
      transform.position += moveDirection * moveSpeed * Time.deltaTime;

      float rotateSpeed = 10f;
      transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

      unitAnimator.SetBool("isWalking", true);
    }

  }

  public void Move(Vector3 targetPosition)
  {
    this.targetPosition = targetPosition;
  }


}
