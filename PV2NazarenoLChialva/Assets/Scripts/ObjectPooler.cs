using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    [Header("Configuración de Pools")]
    public PoolableObjectData[] pools;

    
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePools();
    }

    private void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (PoolableObjectData pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false); 
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.poolTag, objectPool);
        }
    }


    public GameObject GetPooledObject(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool con tag " + tag + " no existe.");
            return null;
        }

        PoolableObjectData poolData = GetPoolDataByTag(tag);
        if (poolData == null) return null;

        if (poolDictionary[tag].Count == 0)
        {

            if (poolData.shouldExpand)
            {
                GameObject newObj = Instantiate(poolData.prefab);

                newObj.transform.position = position;
                newObj.transform.rotation = rotation;
                newObj.SetActive(true);
                return newObj;
            }
            else
            {
                Debug.LogWarning("Pool con tag " + tag + " está vacío y no puede expandirse.");
                return null;
            }
        }


        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true); 

        return objectToSpawn;
    }

    public void ReturnObjectToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Destroy(objectToReturn);
            return;
        }

        objectToReturn.SetActive(false); 
        poolDictionary[tag].Enqueue(objectToReturn);
    }

    private PoolableObjectData GetPoolDataByTag(string tag)
    {
        foreach (PoolableObjectData poolData in pools)
        {
            if (poolData.poolTag == tag)
            {
                return poolData;
            }
        }
        return null;
    }
}