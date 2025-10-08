using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorObjeto : MonoBehaviour
{
    [SerializeField] private GameObject objetoPrefab;

    [SerializeField]
    [Range(0.5f, 5f)]
    private float tiempoEspera;

  
    public Transform playerTarget;

    void Start()
    {
       
        if (playerTarget == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            if (playerGO != null)
            {
                playerTarget = playerGO.transform;
            }
        }

        Invoke("GenerarObjeto", tiempoEspera);
    }

    void GenerarObjeto()
    {
     
        GameObject newEnemyGO = Instantiate(objetoPrefab, transform.position, Quaternion.identity);

        
        EnemyController newEnemyController = newEnemyGO.GetComponent<EnemyController>();

        
        if (newEnemyController != null && playerTarget != null)
        {
            
            newEnemyController.player = playerTarget;
        }
        else
        {
            Debug.LogWarning("No se pudo asignar el target del jugador al enemigo generado.");
        }
    }
}