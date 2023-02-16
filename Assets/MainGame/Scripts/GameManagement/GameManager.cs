using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _Instance { get; private set; }

    public enum GameState { MainMenu, InGame, Pause, GameOver };
    public GameState gameState = GameState.MainMenu;

    private void Awake()
    {
        if (_Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _Instance = this;
        }
    }
}
