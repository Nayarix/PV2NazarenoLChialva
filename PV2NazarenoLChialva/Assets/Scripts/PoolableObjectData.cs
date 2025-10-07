using UnityEngine;

[CreateAssetMenu(fileName = "PoolData", menuName = "Generation/Poolable Object Data")]
public class PoolableObjectData : ScriptableObject
{
    [Header("Objeto a Generar")]
    public GameObject prefab;

    [Header("Configuración de Pool")]
    public int poolSize = 10; 
    public bool shouldExpand = true; 

    [Header("Puntos de Generación")]
    public Vector3[] spawnPoints; 

    [Header("Tiempo de Generación")]
    public float timeToSpawn = 5.0f; 

    [Tooltip("El nombre Tag que identifica a este pool (Ej: 'EnemyPool')")]
    public string poolTag;
}