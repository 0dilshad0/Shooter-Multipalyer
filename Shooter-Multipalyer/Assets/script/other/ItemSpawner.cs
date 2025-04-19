using System.Collections;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] Items;
    public Transform[] Positions;
    public float Time;

    void Start()
    {
        SpawnItems();
        StartCoroutine(ReSpawn());
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
                    Destroy(child.gameObject);
                }

              
            }
            GameObject RandonItem = Items[Random.Range(0, Items.Length)];

            GameObject Item = Instantiate(RandonItem, point.position, Quaternion.identity);

            Item.transform.SetParent(point);



        }
    }
}
