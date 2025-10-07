using UnityEngine;

[CreateAssetMenu(fileName = "PoolData", menuName = "Generation/Poolable Object Data")]
public class PoolableObjectData : ScriptableObject
{
    [Header("Objeto a Generar")]
    public GameObject prefab;

    [Header("Configuraci�n de Pool")]
    public int poolSize = 10; 
    public bool shouldExpand = true; 

    [Header("Puntos de Generaci�n")]
    public Vector3[] spawnPoints; 

    [Header("Tiempo de Generaci�n")]
    public float timeToSpawn = 5.0f; 

    [Tooltip("El nombre Tag que identifica a este pool (Ej: 'EnemyPool')")]
    public string poolTag;
}