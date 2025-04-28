using Photon.Pun;
using UnityEngine;

public class PlaayerSpawner : MonoBehaviourPunCallbacks
{
    public GameObject Player;
    public Transform[] SpawnPoints;

    void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        int index = Random.Range(0, SpawnPoints.Length - 1);
        Transform transform = SpawnPoints[index];
        PhotonNetwork.Instantiate(Player.name, transform.position, Quaternion.identity);
    }
    

    
  
}
