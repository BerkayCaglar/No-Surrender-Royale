using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;
    [SerializeField] private List<GameObject> _playerTowers = new List<GameObject>();
    [SerializeField] private List<GameObject> _enemyTowers = new List<GameObject>();

    public List<GameObject> PlayerTowers
    {
        get => _playerTowers;
        set => _playerTowers = value;
    }
    public List<GameObject> EnemyTowers
    {
        get => _enemyTowers;
        set => _enemyTowers = value;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
