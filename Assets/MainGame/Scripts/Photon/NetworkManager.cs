using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Bolt;

[BoltGlobalBehaviour(BoltNetworkModes.Server)]
public class NetworkManager : GlobalEventListener
{
    [Header("Player1 Camera Properties")]
    [Space(10)]
    private Vector3 _player1CameraPosition = new Vector3(0, 35f, -25f);
    private Quaternion _player1CameraRotation = Quaternion.Euler(55f, 0, 0);

    [Header("Player2 Camera Properties")]
    [Space(10)]
    private Vector3 _player2CameraPosition = new Vector3(0, 35f, 25f);
    private Quaternion _player2CameraRotation = Quaternion.Euler(55f, 180f, 0);

    [Header("Server Camera Properties")]
    [Space(10)]
    private Vector3 _serverCameraPosition = new Vector3(0, 40f, 0);
    private Quaternion _serverCameraRotation = Quaternion.Euler(90f, 0, 0);

    private int _playerCount;
    private float _countdownTime = 99f;


    public override void Connected(BoltConnection connection)
    {
        _playerCount++;
        var evnt = PlayerCountAndConfigureEvent.Create();

        evnt.PlayerCount = _playerCount;

        // If player count is 1, then we will set the camera position and rotation to player1 camera position and rotation
        // Else player count is 2, then we will set the camera position and rotation to player2 camera position and rotation
        evnt.CameraPosition = _playerCount == 1 ? _player1CameraPosition : _player2CameraPosition;
        evnt.CameraRotation = _playerCount == 1 ? _player1CameraRotation : _player2CameraRotation;

        evnt.Send();

        if (_playerCount == 2)
        {
            // Call the StartGame method
            StartCoroutine(Countdown());

            SendStartGameEvent();
        }
        else
        {
            SendPauseGameEvent();
        }
    }
    public override void SceneLoadLocalDone(string scene, IProtocolToken token)
    {
        if (UIManager.Instance == null) return;

        UIManager.Instance.ConfigureTheServerRpc(_serverCameraPosition, _serverCameraRotation);
    }
    public override void OnEvent(SpawnObjectEvent evnt)
    {
        BoltNetwork.Instantiate(evnt.PrefabID, evnt.PrefabPosition, evnt.PrefabRotation);
    }
    public override void OnEvent(DestroyObjectEvent evnt)
    {
        BoltNetwork.Destroy(BoltNetwork.FindEntity(evnt.NetworkIDToDestroy).gameObject);
    }
    public void SendStartGameEvent()
    {
        GameManager.Instance._gameState = GameManager.GameState.Game;

        var evnt = StartGameEvent.Create();
        evnt.GameStarted = true;
        evnt.Send();
    }
    public void SendPauseGameEvent()
    {
        GameManager.Instance._gameState = GameManager.GameState.Pause;

        var evnt = StartGameEvent.Create();
        evnt.GameStarted = false;
        evnt.Send();
    }
    private IEnumerator Countdown()
    {
        // This loop will run until the countdown time is less than or equal to 0
        while (_countdownTime > 0)
        {
            // Wait for 1 second
            yield return new WaitForSeconds(1f);

            // Decrease the countdown time by 1
            _countdownTime--;

            var evnt = CountDownTimerEvent.Create();
            evnt.CountDown = _countdownTime;
            evnt.Send();

            /*
            // If the countdown time is less than or equal to 0, call the TimeIsUp method
            if (_countdownTime <= 0)
            {
                TimeIsUp();
            }
            */
        }
    }
}
