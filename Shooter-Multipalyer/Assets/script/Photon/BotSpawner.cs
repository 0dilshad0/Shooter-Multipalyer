using Photon.Pun;
using UnityEngine;

public class BotSpawner : MonoBehaviourPunCallbacks
{
    public GameObject Bot;
    public Transform pos;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            spawn();
        }
            
    }

    private void spawn()
    {
        PhotonNetwork.Instantiate(Bot.name,pos.position,Quaternion.identity);
    }
   
}
