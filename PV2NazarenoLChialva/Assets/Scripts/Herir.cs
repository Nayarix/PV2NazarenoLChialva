using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herir : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] float puntos = 5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Llama al script PlayerController
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            if (player != null)
            {
                player.RecibirDanio(puntos); // Llama a la nueva funci�n
                Debug.Log("PUNTOS DE DA�O REALIZADOS AL JUGADOR " + puntos);
            }
        }
    }
}