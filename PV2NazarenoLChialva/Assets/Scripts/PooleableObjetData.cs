using UnityEngine;

// Permite crear archivos de configuraci�n desde el men� de Unity
[CreateAssetMenu(fileName = "PoolData", menuName = "Pooling/Poolable Object Data", order = 1)]
public class PoolableObjectData : ScriptableObject
{
    [Header("Configuraci�n del Pool")]
    [Tooltip("Etiqueta para identificar esta piscina (ej: 'Enemigo')")]
    public string poolTag = "Enemigo";

    [Tooltip("El Prefab que se va a clonar en la piscina.")]
    public GameObject prefab;

    [Tooltip("N�mero inicial de objetos en la piscina.")]
    public int poolSize = 5;

    [Tooltip("Permitir que la piscina cree nuevos objetos si se agota.")]
    public bool shouldExpand = true;

    // --- Configuraci�n de Spawn (Usada por el Spawner) ---
    [Header("Configuraci�n de Spawn (Opcional)")]
    [Tooltip("Tiempo de pausa entre la generaci�n de cada enemigo.")]
    public float timeBetweenSpawns = 2.0f;

    [Tooltip("Puntos de la escena donde se pueden generar los enemigos.")]
    public Vector3[] spawnPoints;
}