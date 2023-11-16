using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public InputField input;
    public Transform chatContent;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            input.ActivateInputField();

            if (input.text.Length == 0) return;

            string chat = PhotonNetwork.NickName + " : " + input.text;

            photonView.RPC(nameof(Chatting), RpcTarget.All, chat);
        }

    }

    [PunRPC]
    void Chatting(string msg)
    {
        // chatPrefab 오브젝트를 하나 만들어서 text값에 저장합니다.
        GameObject chat = Instantiate(Resources.Load<GameObject>("String"));

        chat.GetComponent<Text>().text = msg;

        // 스크롤 뷰 - content 오브젝트의 자식으로 등록합니다.
        chat.transform.SetParent(chatContent);

        // 채팅을 입력한 후에도 이어서 입력할 수 있도록 설정하는 함수입니다.
        input.ActivateInputField();

        // 채팅을 입력한 후에 초기화합니다.
        input.text = null;

    }



}
