using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance;

    [System.Serializable]
    public class Pool
    {
        public string name;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> PoolList;
    private Dictionary<string, Queue<GameObject>> PoolDict;

    private void Awake()
    {
        Instance = this;
        PoolDict = new Dictionary<string, Queue<GameObject>>();

        foreach(Pool pool in PoolList)
        {
            Queue<GameObject> objQueue = new Queue<GameObject>();

            for(int i=0; i<pool.size;i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objQueue.Enqueue(obj);

            }
            PoolDict.Add(pool.name, objQueue);
        }
    }
    public GameObject spawn(string name,Vector3 position,quaternion rotation)
    {
        if (!PoolDict.ContainsKey(name)) return null;

        GameObject obj = PoolDict[name].Dequeue();
        obj.SetActive(true);
        obj.transform.SetLocalPositionAndRotation(position, rotation);
        PoolDict[name].Enqueue(obj);

        return obj;
    }
}
