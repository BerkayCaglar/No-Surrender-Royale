using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is used to manage all the towers in the game. It is a singleton class.
public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;
    [SerializeField] private List<GameObject> _playerTowers = new List<GameObject>();
    [SerializeField] private List<GameObject> _enemyTowers = new List<GameObject>();

    private int _enemyTowerLayer = 10;
    private int _playerTowerLayer = 3;

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
    public int EnemyTowerLayer
    {
        get => _enemyTowerLayer;
    }
    public int PlayerTowerLayer
    {
        get => _playerTowerLayer;
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
