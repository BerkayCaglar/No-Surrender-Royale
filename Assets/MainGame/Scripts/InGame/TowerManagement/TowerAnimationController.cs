using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAnimationController : MonoBehaviour
{
    private Tower _tower;
    private void Start()
    {
        _tower = GetComponent<Tower>();
    }
    public void PlayAttackAnimation()
    {
        if (CompareTag("PlayerMainTower") || CompareTag("EnemyMainTower"))
        {
            _tower.Animator.SetTrigger("MainTowerAttack");
            return;
        }
        else if (CompareTag("PlayerLeftTower") || CompareTag("EnemyLeftTower"))
        {
            _tower.Animator.SetTrigger("LeftTowerAttack");
            return;
        }
        else if (CompareTag("PlayerRightTower") || CompareTag("EnemyRightTower"))
        {
            _tower.Animator.SetTrigger("RightTowerAttack");
            return;
        }
    }
}
