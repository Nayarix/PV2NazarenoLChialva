using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetJuego : MonoBehaviour
{
    public KeyCode teclaReset = KeyCode.R; 
    public bool resetAlTocar = true; 

    private void Update()
    {
        
        if (Input.GetKeyDown(teclaReset))
        {
            ResetearEscena();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (resetAlTocar && collision.CompareTag("Player"))
        {
            ResetearEscena();
        }
    }

    public void ResetearEscena()
    {
        
        int escenaActual = SceneManager.GetActiveScene().buildIndex;

        
        SceneManager.LoadScene(escenaActual);

        Debug.Log("Juego reseteado - Escena recargada");
    }

    
    public void ResetearJuegoCompleto()
    {
        ResetearEscena();
    }
}