using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public Button roomCreate;
    public InputField roomName;
    public InputField roomPerson;
    public Transform roomContent;

    // 룸 목록을 저장하기 위한 자료구조
    Dictionary<string, RoomInfo> roomCatalog = new Dictionary<string, RoomInfo>();

    // Update is called once per frame
    void Update()
    {
        if(roomName.text.Length > 0 && roomPerson.text.Length > 0)
        {
            roomCreate.interactable = true;
        }
        else
        {
            roomCreate.interactable = false;
        }

    }


    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Photon Game");
    }


    public void CreateRoomObject()
    {
        foreach(RoomInfo info in roomCatalog.Values)
        {
            // 룸 오브젝트를 생성합니다.
            GameObject room = Instantiate(Resources.Load<GameObject>("Room"));

            // roomCatalog의 하위 오브젝트를 생성합니다.
            room.transform.SetParent(roomContent);

            // 룸에 대한 정보를 입력합니다.
            room.GetComponent<Information>().SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);

        }

    }
}
