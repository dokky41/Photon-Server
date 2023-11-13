using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ConnectServer : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Dropdown server;

    private void Awake()
    {
        server.options[0].text = "Union";
        server.options[1].text = "Aether";
        server.options[2].text = "Haselo";
    }

    public void SelectServer()
    {
        // 서버 접속.
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.LoadLevel("Photon Room");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby
        (
            new TypedLobby
            (
                server.options[server.value].text,
                LobbyType.Default
            )

        );
    }


}
    
