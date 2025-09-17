using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject luzCheckpoint; 
    public AudioClip sonidoActivacion;

    private bool estaActivado = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !estaActivado)
        {
            Jugador jugador = collision.GetComponent<Jugador>();
            if (jugador != null)
            {
                jugador.EstablecerPuntoRespawn(transform.position);
                estaActivado = true;

                
                if (luzCheckpoint != null)
                {
                    luzCheckpoint.SetActive(true);
                }

                if (sonidoActivacion != null)
                {
                    AudioSource.PlayClipAtPoint(sonidoActivacion, transform.position);
                }

                Debug.Log("Checkpoint activado!");
            }
        }
    }
}