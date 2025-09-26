using UnityEngine;

public class BloqueMortal : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Jugador jugador = other.GetComponent<Jugador>();

            PlayerController controller = other.GetComponent<PlayerController>();

            if (jugador != null && controller != null)
            {
                float danioMortal = jugador.vidaActual;

                controller.RecibirDanio(danioMortal);

                Debug.Log("¡Bloque Mortal activado! Daño infligido: " + danioMortal);
            }
        }
    }
}