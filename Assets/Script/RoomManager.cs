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

    // �� ����� �����ϱ� ���� �ڷᱸ��
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
            // �� ������Ʈ�� �����մϴ�.
            GameObject room = Instantiate(Resources.Load<GameObject>("Room"));

            // roomCatalog�� ���� ������Ʈ�� �����մϴ�.
            room.transform.SetParent(roomContent);

            // �뿡 ���� ������ �Է��մϴ�.
            room.GetComponent<Information>().SetInfo(info.Name, info.PlayerCount, info.MaxPlayers);

        }

    }

    public void OnClickCreateRoom()
    {
        // �� �ɼ��� �����մϴ�.
        RoomOptions room = new RoomOptions();

        // �ִ� �������� ���� �����մϴ�.
        room.MaxPlayers = byte.Parse(roomPerson.text);

        // ���� ���� ���θ� �����մϴ�.
        room.IsOpen = true;

        // �κ񿡼� ���� �����ų �� �����մϴ�.
        room.IsVisible = true;

        // ���� �����ϴ� �Լ�
        PhotonNetwork.CreateRoom(roomName.text, room);


    }

    public void AllDeleteRoom()
    {
        foreach(Transform trans in roomContent)
        {
            Destroy(trans.gameObject);
        }
    }

    // �ش� �κ� �� ����� ���� ������ ������ ȣ��Ǵ� �Լ�(�߰�,����,����)
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
            // �ش� �̸��� roomCatalog�� key�� �����Ѵٸ� 
            if (roomCatalog.ContainsKey(roomList[i].Name))
            {
                // roomList[i].RemovedFromList : (true) �뿡�� ������ �Ǿ��� ��
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
