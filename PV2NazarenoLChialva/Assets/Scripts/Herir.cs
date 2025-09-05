using UnityEngine;

public class HerirEnemigo : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] float puntosDeDanio = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemigo"))
        {
            EnemyController enemigo = other.gameObject.GetComponent<EnemyController>();

            if (enemigo != null)
            {
                enemigo.RecibirDanio(puntosDeDanio);
                Debug.Log("¡Enemigo dañado!");
            }
        }
    }
}