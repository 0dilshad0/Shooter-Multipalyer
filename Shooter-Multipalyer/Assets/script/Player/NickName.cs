using Photon.Pun;
using TMPro;
using UnityEngine;

public class NickName : MonoBehaviour
{
    private PhotonView photonView;
    public TMP_Text Name;
    public GameObject obj;
    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (photonView.IsMine)
        {
            photonView.RPC("SetNickName", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer.NickName);
        }
       
    }

    
    void Update()
    {
        if (photonView.IsMine) 
        {
            Name.enabled = false;
        }
        else
        {
           obj.transform.LookAt(Camera.main.transform);
        }

        
    }

    [PunRPC]
    private void SetNickName(string name)
    {
        Name.text = name;
    }

     
}
