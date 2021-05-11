using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string objectTag;
        public GameObject objectPrefab;
        public int amountToPool;
    }

    public static ObjectPooler Instance;
    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        MakeInstance();

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.amountToPool; i++)
            {
                GameObject obj = Instantiate(pool.objectPrefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            poolDictionary.Add(pool.objectTag, objectPool);
        }
    }

    void MakeInstance()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetPooledObject(string reqTag, Vector3 reqPosition, Quaternion reqRotation)
    {
        if (!poolDictionary.ContainsKey(reqTag))
        {
            Debug.LogWarning($"Pool with tag {reqTag} doesn't exist");
            return null;
        }
        GameObject objectToSpawn = poolDictionary[reqTag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = reqPosition;
        objectToSpawn.transform.rotation = reqRotation;

        poolDictionary[reqTag].Enqueue(objectToSpawn);

        //Debug.Log("Returned object to pool with tag: " + reqTag);

        return objectToSpawn;
    }
}