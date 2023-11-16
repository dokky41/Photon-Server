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
        // chatPrefab ������Ʈ�� �ϳ� ���� text���� �����մϴ�.
        GameObject chat = Instantiate(Resources.Load<GameObject>("String"));

        chat.GetComponent<Text>().text = msg;

        // ��ũ�� �� - content ������Ʈ�� �ڽ����� ����մϴ�.
        chat.transform.SetParent(chatContent);

        // ä���� �Է��� �Ŀ��� �̾ �Է��� �� �ֵ��� �����ϴ� �Լ��Դϴ�.
        input.ActivateInputField();

        // ä���� �Է��� �Ŀ� �ʱ�ȭ�մϴ�.
        input.text = null;

    }



}
