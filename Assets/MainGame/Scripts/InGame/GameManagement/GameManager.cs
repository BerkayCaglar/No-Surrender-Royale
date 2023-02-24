using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

public class GameManager : GlobalEventListener
{
    public static GameManager Instance { get; private set; }

    public Camera _mainCamera;
    public TowerManager _towerManager;
    public int _playerCount;
    public GameObject _battleMap;
    private bool _towersAttached = false;

    [SerializeField] private List<GameObject> _player1Deck = new List<GameObject>();
    [SerializeField] private List<GameObject> _player2Deck = new List<GameObject>();

    public List<GameObject> Player1Deck
    {
        get => _player1Deck;
    }
    public List<GameObject> Player2Deck
    {
        get => _player2Deck;
    }

    public enum GameState
    {
        MainMenu,
        Updating,
        Game,
        Pause,
        GameOver,
        WaitingForPlayers,
    }
    public GameState _gameState;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        Application.targetFrameRate = Screen.currentResolution.refreshRate;
    }

    public void SetCameraPositionAndRotation(Vector3 position, Quaternion rotation)
    {
        _mainCamera.transform.position = position;
        _mainCamera.transform.rotation = rotation;
    }
    public override void OnEvent(PlayerCountAndConfigureEvent evnt)
    {
        _playerCount = evnt.PlayerCount;
    }
    private void Update()
    {
        /*
        if ((_towerManager != null && BoltNetwork.IsServer && _playerCount == 2) && !_towersAttached)
        {
            BoltNetwork.Attach(_battleMap);

            foreach (var tower in _towerManager.EnemyTowers)
            {
                BoltNetwork.Attach(tower);
                tower.transform.SetParent(_battleMap.transform);
            }

            foreach (var tower in _towerManager.PlayerTowers)
            {
                BoltNetwork.Attach(tower);
                tower.transform.SetParent(_battleMap.transform);
            }
            _towersAttached = true;
        }
        */
    }
}
