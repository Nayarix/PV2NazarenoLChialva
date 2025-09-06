using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jugador : MonoBehaviour
{
    
    [Header("Configuracion")]
    public float vida = 15.0f;

    public void ModificarVida(float puntos)
    {
        vida += puntos;
        Debug.Log("Vida actual del jugador: " + vida);
    }

    public bool EstasVivo()
    {
        return vida > 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Meta")) { return; }

        Debug.Log("GANASTE");
    }
}