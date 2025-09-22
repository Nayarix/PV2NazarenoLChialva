using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    [Header("Configuracion")]
    public float vidaActual = 15.0f; 
    public float vidaMaxima = 15.0f;
    public float danioAtaque = 5.0f;

    [Header("Respawn")]
    public Vector3 puntoRespawn;
    private Vector3 posicionInicial;

    void Start()
    {
        posicionInicial = transform.position;
        puntoRespawn = posicionInicial;

       
        vidaActual = vidaMaxima;
    }

    
    public void ModificarVida(float puntos)
    {
        vidaActual += puntos;

        if (vidaActual > vidaMaxima)
        {
            vidaActual = vidaMaxima;
        }

        Debug.Log("Vida actual del jugador: " + vidaActual);
    }

    
    public void ModificarDanio(float cantidad)
    {
        danioAtaque += cantidad;
    }

    public bool EstasVivo()
    {
        return vidaActual > 0;
    }

    public void Respawn()
    {
        if (!EstasVivo())
        {
            vidaActual = vidaMaxima;
            transform.position = puntoRespawn;

            PlayerController playerController = GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.ReactivarJugador();
            }

            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("Muerto", false);
                animator.Play("Idle", 0, 0f);
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