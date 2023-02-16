using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TowerUIManager))]
public class Tower : MonoBehaviour
{
    private float _health;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _detectRange;

    public float Health
    {
        get => _health;
        set { _health = Mathf.Clamp(value, 0f, _maxHealth); }
    }
    public float MaxHealth
    {
        get => _maxHealth;
    }
    private void Start()
    {
        Health = MaxHealth;
        InvokeRepeating("CheckTowerHealthAndDecide", 0f, 0.5f);
    }
    private void CheckTowerHealthAndDecide()
    {
        if (Health <= 0f)
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
                UIManager._Instance.YouWin();
            }
            else if (CompareTag("PlayerMainTower"))
            {
                UIManager._Instance.YouLose();
            }
            Destroy(gameObject);
        }
    }
}
