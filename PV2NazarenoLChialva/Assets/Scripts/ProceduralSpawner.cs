using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralSpawner : MonoBehaviour
{
    [Header("Configuración del Pool")]
    public PoolableObjectData enemyPoolData; 

    private void Start()
    {
       
        if (ObjectPooler.Instance != null && enemyPoolData != null)
        {
           
            StartCoroutine(SpawnRoutine());
        }
        else
        {
            Debug.LogError("Falta configurar el ObjectPooler.Instance o el enemyPoolData en el Inspector.");
        }
    }

    private IEnumerator SpawnRoutine()
    {
        
        while (true)
        {
        
            yield return new WaitForSeconds(enemyPoolData.timeToSpawn);

    
            for (int i = 0; i < enemyPoolData.spawnPoints.Length; i++)
            {
                Vector3 spawnPosition = enemyPoolData.spawnPoints[i];

                ObjectPooler.Instance.GetPooledObject(
                    enemyPoolData.poolTag,
                    spawnPosition,
                    Quaternion.identity
                );
            }
        }
    }
}