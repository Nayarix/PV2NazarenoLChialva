using UnityEngine;

public class HerirEnemigo : MonoBehaviour
{
    private Jugador jugador;

    void Start()
    {
        jugador = GetComponentInParent<Jugador>();

        if (jugador == null)
        {
            Debug.LogError("Error: el script 'HerirEnemigo' no encontró el componente 'Jugador' en el objeto padre. " +
                           "Asegúrate de que este script esté en un hijo del objeto del jugador.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemigo"))
        {
            EnemyController enemigo = other.gameObject.GetComponent<EnemyController>();

            if (enemigo != null && jugador != null)
            {
                enemigo.RecibirDanio(jugador.danioAtaque);
                Debug.Log("¡Enemigo dañado con " + jugador.danioAtaque + " puntos de daño!");
            }
        }
    }
}