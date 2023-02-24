using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Photon.Bolt;
using Photon.Bolt.Matchmaking;
using UdpKit;

public class BoltUIManager : GlobalEventListener
{
    // Called from HostGameButton
    public void StartServer()
    {
        BoltLauncher.StartServer();
    }

    public override void BoltStartDone()
    {
        if (!BoltNetwork.IsServer) return;

        BoltMatchmaking.CreateSession(sessionID: "My Session", sceneToLoad: "BattleScene");
    }

    // Called from JoinGameButton
    public void StartClient()
    {
        BoltLauncher.StartClient();
    }

    public override void SessionListUpdated(Map<Guid, UdpSession> sessionList)
    {
        foreach (var session in sessionList)
        {
            UdpSession photonSession = session.Value as UdpSession;

            if (photonSession.Source == UdpSessionSource.Photon)
            {
                BoltMatchmaking.JoinSession(photonSession);
            }
        }
    }
}
