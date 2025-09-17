using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    [Header("Configuracion")]
    public float vida = 15.0f;
    public float vidaMaxima = 15.0f;

    [Header("Respawn")]
    public Vector3 puntoRespawn; 
    private Vector3 posicionInicial;

    void Start()
    {
        
        posicionInicial = transform.position;
        puntoRespawn = posicionInicial;

        
        vidaMaxima = vida;
    }

    public void ModificarVida(float puntos)
    {
        vida += puntos;

        
        if (vida > vidaMaxima)
        {
            vida = vidaMaxima;
        }

        Debug.Log("Vida actual del jugador: " + vida);
    }

    public bool EstasVivo()
    {
        return vida > 0;
    }

    public void Respawn()
    {
        if (!EstasVivo())
        {
            // Restaurar vida
            vida = vidaMaxima;

            // Reposicionar al jugador en el punto de respawn
            transform.position = puntoRespawn;

            // Reactivar componentes a través del PlayerController
            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ReactivarJugador();
            }

            // Resetear solo la animación de muerte
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("Muerto", false);
                animator.Play("Idle", 0, 0f); // Forzar animación idle
            }

            Debug.Log("Jugador ha reaparecido con vida completa");
        }
    }

    private void ResetearAnimaciones()
    {
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            
            animator.SetBool("Muerto", false);
            animator.SetBool("RecibeDanio", false);
            animator.SetBool("Atacando", false);
            animator.SetBool("EnSuelo", true); 

            
            animator.ResetTrigger("Ataque"); 

            
            animator.Play("Idle", 0, 0f); 
        }
    }

    public void EstablecerPuntoRespawn(Vector3 nuevoPunto)
    {
        puntoRespawn = nuevoPunto;
        Debug.Log("Nuevo punto de respawn establecido: " + nuevoPunto);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Meta")) { return; }

        Debug.Log("GANASTE");
    }
}