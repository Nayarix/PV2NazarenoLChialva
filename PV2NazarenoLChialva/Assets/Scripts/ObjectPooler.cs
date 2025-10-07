using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance;

    [System.Serializable]
    public class Pool
    {
        public PoolableObjectData data;

        [HideInInspector]
        public Queue<GameObject> objectPool;
    }

    public List<Pool> pools; 
    private Dictionary<string, Pool> poolDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        poolDictionary = new Dictionary<string, Pool>();

        foreach (Pool pool in pools)
        {
            pool.objectPool = new Queue<GameObject>();

            if (!poolDictionary.ContainsKey(pool.data.poolTag))
            {
                poolDictionary.Add(pool.data.poolTag, pool);
            }

            
            for (int i = 0; i < pool.data.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.data.prefab);
                obj.SetActive(false); 
                pool.objectPool.Enqueue(obj); 
            }
        }
    }

   
    public GameObject GetPooledObject(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool con tag " + tag + " no existe.");
            return null;
        }

        Pool pool = poolDictionary[tag];

        if (pool.objectPool.Count == 0)
        {
            if (pool.data.shouldExpand)
            {
           
                GameObject newObj = Instantiate(pool.data.prefab);

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

       
        GameObject objectToSpawn = pool.objectPool.Dequeue();

      
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);


        return objectToSpawn;
    }

    
    public void ReturnObjectToPool(string tag, GameObject objectToReturn)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool con tag " + tag + " no existe.");
            
            Destroy(objectToReturn);
            return;
        }

        objectToReturn.SetActive(false);
        poolDictionary[tag].objectPool.Enqueue(objectToReturn);
    }
}