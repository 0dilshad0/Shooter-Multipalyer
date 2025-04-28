using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ItemSpawner : MonoBehaviourPunCallbacks
{
    public GameObject[] Items;
    public Transform[] Positions;
    public float Time;

    void Start()
    {
        if(PhotonNetwork.IsMasterClient)
        {
            SpawnItems();
            StartCoroutine(ReSpawn());
        }
       
    }


  

    IEnumerator ReSpawn()
    {
        yield return new WaitForSecondsRealtime(Time);

        SpawnItems();
        StartCoroutine(ReSpawn());
        
    }


    private void SpawnItems()
    {
        foreach (Transform point in Positions)
        {
            if(point.childCount >0)
            {
                foreach(Transform child in point)
                {
                    PhotonNetwork.Destroy(child.gameObject);
                }

              
            }
            GameObject RandonItem = Items[Random.Range(0, Items.Length)];

            GameObject Item = PhotonNetwork.Instantiate("picUp/" + RandonItem.name, point.position, Quaternion.identity);

            Item.transform.SetParent(point);



        }
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        SpawnItems();
        StartCoroutine(ReSpawn());
    }
}
