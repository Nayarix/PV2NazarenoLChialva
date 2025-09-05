using UnityEngine;

public class HerirJugador : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] float puntosDeDanio = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Player"))
        {
            
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                player.RecibirDanio(puntosDeDanio); 
                Debug.Log("PUNTOS DE DAÑO REALIZADOS AL JUGADOR " + puntosDeDanio);
            }
        }
    }
}