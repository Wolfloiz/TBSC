using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
  [SerializeField] Animator animator;
  [SerializeField] private Transform bulletProjectilePrefab;

  [SerializeField] private Transform shootPointTransform;
  [SerializeField] private Transform swordTransform;
  [SerializeField] private Transform rifleTransform;

  private void Awake()
  {
    if (TryGetComponent<MoveAction>(out MoveAction moveAction))
    {
      moveAction.OnStartMoving += MoveAction_OnStartMoving;
      moveAction.OnStopMoving += MoveAction_OnStopMoving;
    }

    if (TryGetComponent<ShootAction>(out ShootAction shootAction))
    {
      shootAction.OnShoot += ShootAction_OnShoot;
    }

    if (TryGetComponent<SwordAction>(out SwordAction swordAction))
    {
      swordAction.OnSwordActionStarted += SwordAction_OnSwordActionStarted;
      swordAction.OnSwordActionCompleted += SwordAction_OnSwordActionCompleted;
    }
  }

  private void Start()
  {
    EquipRifle();
  }

  private void SwordAction_OnSwordActionCompleted(object sender, EventArgs e)
  {
    EquipRifle();
  }

  private void SwordAction_OnSwordActionStarted(object sender, EventArgs e)
  {
    EquipSword();
    animator.SetTrigger("SwordSlash");
  }

  private void MoveAction_OnStartMoving(object sender, EventArgs e)
  {
    animator.SetBool("isWalking", true);
  }

  private void MoveAction_OnStopMoving(object sender, EventArgs e)
  {
    animator.SetBool("isWalking", false);
  }

  private void ShootAction_OnShoot(object sender, ShootAction.OnShootEventArgs e)
  {
    animator.SetTrigger("Shoot");

    Transform bulletProjectileTranform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
    BulletProjectile bulletProjectile = bulletProjectileTranform.GetComponent<BulletProjectile>();

    Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();

    targetUnitShootAtPosition.y = shootPointTransform.position.y;

    bulletProjectile.Setup(targetUnitShootAtPosition);

  }

  private void EquipSword()
  {
    rifleTransform.gameObject.SetActive(false);
    swordTransform.gameObject.SetActive(true);

  }

  private void EquipRifle()
  {
    swordTransform.gameObject.SetActive(false);
    rifleTransform.gameObject.SetActive(true);
  }

}
