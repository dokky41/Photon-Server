using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonPlayer : MonoBehaviourPunCallbacks, IPunObservable
{
    [SerializeField] TextMeshProUGUI nickName;

    [SerializeField] float score;
    [SerializeField] float mouseX;
    [SerializeField] float rotateSpeed;
    [SerializeField] float speed = 5.0f;

    [SerializeField] Vector3 direction;
    [SerializeField] Camera temporaryCamera;

    void Awake()
    {
        nickName.text = photonView.Owner.NickName;
    }

    void Start()
    {
        // ���� �÷��̾ �� �ڽ��̶��
        if(photonView.IsMine)
        {
            Camera.main.gameObject.SetActive(false);
        }
        else
        {
            temporaryCamera.enabled = false;
            GetComponentInChildren<AudioListener>().enabled = false;
        }
    }

    void Update()
    {
        if(!photonView.IsMine) return;

        MoveMent(Time.deltaTime);

        mouseX += Input.GetAxis("Mouse X") * rotateSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0, mouseX, 0);

    }



    public void MoveMent(float time)
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        direction.Normalize();

        transform.position += transform.TransformDirection(direction) * speed * time;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        // ���� ������Ʈ��� ���� �κ��� �����մϴ�.
        if(stream.IsWriting)
        {
            stream.SendNext(score);
        }
        else // ���� ������Ʈ��� �ϸ� �б� �κ��� �����մϴ�.
        {
            // ��Ʈ��ũ�� ���ؼ� �����͸� �޽��ϴ�.
            score = (float)stream.ReceiveNext();
        }


    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Preasure Box"))
        {
            PhotonView view = other.GetComponent<PhotonView>();

            if(view.IsMine)
            {
                score++;

                PhotonNetwork.Destroy(view);

            }


        }

    }


}
