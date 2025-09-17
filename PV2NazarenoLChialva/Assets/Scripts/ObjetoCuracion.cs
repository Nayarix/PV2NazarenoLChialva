using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetoCuracion : MonoBehaviour
{
    public float cantidadCuracion = 15f;
    public AudioClip sonidoCuracion;
    public float tiempoReutilizacion = 2f;

    private bool puedeCurar = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && puedeCurar)
        {
            Jugador jugador = collision.GetComponent<Jugador>();
            if (jugador != null && jugador.EstasVivo())
            {
                jugador.ModificarVida(cantidadCuracion);

                if (sonidoCuracion != null)
                {
                    AudioSource.PlayClipAtPoint(sonidoCuracion, transform.position);
                }

                StartCoroutine(ReactivarObjeto());
            }
        }
    }

    private IEnumerator ReactivarObjeto()
    {
        puedeCurar = false;
        yield return new WaitForSeconds(tiempoReutilizacion);
        puedeCurar = true;
    }
}