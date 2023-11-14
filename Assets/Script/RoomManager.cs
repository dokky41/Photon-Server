using Photon.Pun;
using Photon.Realtime;
using System;
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

    public void OnClickCreateRoom()
    {
        // 룸 옵션을 설정합니다.
        RoomOptions room = new RoomOptions();

        // 최대 접속자의 수를 설정합니다.
        room.MaxPlayers = byte.Parse(roomPerson.text);

        // 룸의 오픈 여부를 설정합니다.
        room.IsOpen = true;

        // 로비에서 룸을 노출시킬 지 설정합니다.
        room.IsVisible = true;

        // 룸을 생성하는 함수
        PhotonNetwork.CreateRoom(roomName.text, room);


    }

    public void AllDeleteRoom()
    {
        foreach(Transform trans in roomContent)
        {
            Destroy(trans.gameObject);
        }
    }

    // 해당 로비에 방 목록의 변경 사항이 있으면 호출되는 함수(추가,삭제,참가)
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        AllDeleteRoom();
        UpdateRoom(roomList);
        CreateRoomObject();
    }

    private void UpdateRoom(List<RoomInfo> roomList)
    {
        for(int i=0; i<roomList.Count; i++)
        {
            // 해당 이름의 roomCatalog의 key가 존재한다면 
            if (roomCatalog.ContainsKey(roomList[i].Name))
            {
                // roomList[i].RemovedFromList : (true) 룸에서 삭제가 되었을 때
                if (roomList[i].RemovedFromList) 
                {
                    roomCatalog.Remove(roomList[i].Name);
                    continue;
                }

            }
            roomCatalog[roomList[i].Name] = roomList[i];
        }
    }

  

}
