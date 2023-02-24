using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Client)]
public class PlayerJoined : GlobalEventListener
{
    private bool _isConfigured, _isGameStarted;
    private int _playerCount;
    private Vector3 _cameraPosition;
    private Quaternion _cameraRotation;

    public override void OnEvent(PlayerCountAndConfigureEvent evnt)
    {
        if (_isConfigured) return;

        _playerCount = evnt.PlayerCount;

        _isConfigured = true;

        _cameraPosition = evnt.CameraPosition;
        _cameraRotation = evnt.CameraRotation;
    }
    public override void OnEvent(CountDownTimerEvent evnt)
    {
        UIManager.Instance.SetCountdownText(evnt.CountDown);
    }
    public override void OnEvent(StartGameEvent evnt)
    {
        _isGameStarted = evnt.GameStarted;
    }
    public override void SceneLoadRemoteDone(BoltConnection connection, IProtocolToken token)
    {
        if (_isConfigured)
        {
            UIManager.Instance.ConfigureTheClientRpc(_cameraPosition, _cameraRotation);

            UIManager.Instance.SetPlayerCards(_playerCount == 1 ? true : false);
        }
    }

    private void Update()
    {
        try
        {
            if (GameManager.Instance._gameState != GameManager.GameState.Game)
            {
                if (_isGameStarted)
                {
                    GameManager.Instance._gameState = GameManager.GameState.Game;
                    UIManager.Instance.HideWaitingForPlayersMenu();
                }
                else
                {
                    GameManager.Instance._gameState = GameManager.GameState.WaitingForPlayers;
                    UIManager.Instance.ShowWaitingForPlayersMenu();
                }
            }
        }
        catch (System.Exception)
        {
            Debug.Log("Waiting for players");
        }
    }
}