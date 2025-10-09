using System.Collections;
using UnityEngine;

public class GeneradorObjeto : MonoBehaviour
{
    [Header("Configuración de Generación")]
    [Tooltip("Prefab del enemigo a generar.")]
    [SerializeField] private GameObject objetoPrefab;

    [Tooltip("Tiempo de pausa entre la generación de cada enemigo.")]
    [SerializeField] private float tiempoEntreGeneraciones = 10.0f;

    [HideInInspector] public Transform playerTarget;

    private int enemigosRestantes = 0;

    private void Awake()
    {
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
        if (playerGO != null)
        {
            playerTarget = playerGO.transform;
        }
    }

    public void IniciarGeneracionEnemigos()
    {
        enemigosRestantes = 3;

        StartCoroutine(GeneracionEnemigosRoutine());
    }

    private IEnumerator GeneracionEnemigosRoutine()
    {
        while (enemigosRestantes > 0)
        {
            GenerarObjeto();
            enemigosRestantes--;

            yield return new WaitForSeconds(tiempoEntreGeneraciones);
        }

        Debug.Log("Generación de enemigos completada.");
    }

    private void GenerarObjeto()
    {
        if (objetoPrefab == null)
        {
            Debug.LogError("No hay prefab asignado en el GeneradorObjeto.");
            return;
        }

        GameObject newEnemyGO = Instantiate(objetoPrefab, transform.position, Quaternion.identity);

        EnemyController newEnemyController = newEnemyGO.GetComponent<EnemyController>();

        if (newEnemyController != null && playerTarget != null)
        {
            newEnemyController.player = playerTarget;
        }
        else if (playerTarget == null)
        {
            Debug.LogError("Referencia del jugador no encontrada. Los enemigos no se moverán.");
        }
    }
}