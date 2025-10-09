using System.Collections;
using UnityEngine;

public class ProceduralSpawner : MonoBehaviour
{
    [Header("Configuración del Pool y Spawning")]
    [Tooltip("El Scriptable Object con la configuración del enemigo.")]
    public PoolableObjectData enemyPoolData;

    [Tooltip("El número total de enemigos a generar en esta oleada.")]
    public int totalEnemigosEnOleada = 5;

    private Transform playerTarget;

    private void Awake()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerTarget = playerGO.transform;
        }
        else
        {
            Debug.LogWarning("Jugador no encontrado. Asegúrate de que el personaje tiene el tag 'Player'.");
        }
    }

    public void IniciarGeneracionEnemigos()
    {
        if (enemyPoolData == null)
        {
            Debug.LogError("ERROR: Asigna el PoolableObjectData (GoblinData) al Procedural Spawner.");
            return;
        }
        if (ObjectPooler.Instance != null)
        {
            StartCoroutine(SpawnRoutine());
        }
        else
        {
            Debug.LogError("ERROR: ObjectPooler.Instance no encontrado.");
        }
    }

    private IEnumerator SpawnRoutine()
    {
        int enemigosGenerados = 0;

        while (enemigosGenerados < totalEnemigosEnOleada)
        {
            yield return new WaitForSeconds(enemyPoolData.timeBetweenSpawns);


            Vector3 spawnPosition = enemyPoolData.spawnPoints.Length > 0 ? enemyPoolData.spawnPoints[0] : transform.position;

            GameObject newEnemy = ObjectPooler.Instance.GetPooledObject(
                enemyPoolData.poolTag,
                spawnPosition,
                Quaternion.identity
            );

            if (newEnemy != null)
            {
                EnemyController ec = newEnemy.GetComponent<EnemyController>();
                if (ec != null && playerTarget != null)
                {
                    ec.player = playerTarget;
                }

                enemigosGenerados++;
            }
        }

        Debug.Log("Oleada de " + totalEnemigosEnOleada + " enemigos finalizada.");
    }
}