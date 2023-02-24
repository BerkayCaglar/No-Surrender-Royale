using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerHealthManager : MonoBehaviour
{
    private Tower _tower;
    private void Start()
    {
        _tower = GetComponent<Tower>();

        InvokeRepeating("CheckTowerHealthAndDecide", 0f, 0.5f);
    }
    private void CheckTowerHealthAndDecide()
    {
        if (_tower.Health <= 0f)
        {
            if (gameObject.layer == 10)
            {
                TowerManager.Instance.EnemyTowers.Remove(gameObject);
            }
            else if (gameObject.layer == 3)
            {
                TowerManager.Instance.PlayerTowers.Remove(gameObject);
            }
            if (CompareTag("EnemyMainTower"))
            {
                UIManager.Instance.YouWin();
            }
            else if (CompareTag("PlayerMainTower"))
            {
                UIManager.Instance.YouLose();
            }

            _tower.TowerParticleSystemController.PlayDeathParticleSystem();

            Destroy(gameObject);
        }
    }
}
