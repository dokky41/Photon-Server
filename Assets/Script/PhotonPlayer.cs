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
        // 현재 플레이어가 나 자신이라면
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
        // 로컬 오브젝트라면 쓰기 부분을 실행합니다.
        if(stream.IsWriting)
        {
            stream.SendNext(score);
        }
        else // 원격 오브젝트라고 하면 읽기 부분을 실행합니다.
        {
            // 네트워크를 통해서 데이터를 받습니다.
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
