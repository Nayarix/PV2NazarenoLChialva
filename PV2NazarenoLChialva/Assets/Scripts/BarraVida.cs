using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image RellenobarraVida;
    // Hacemos esta variable pública para asignarla en el Inspector
    public Jugador jugador;
    private float vidaInicial; // Usamos esta variable para guardar la vida inicial

    void Start()
    {
        if (jugador != null)
        {
            vidaInicial = jugador.vida; // Asigna el valor inicial de la vida
        }
    }

    void Update()
    {
        // Añadimos una comprobación de seguridad para evitar el error
        if (jugador != null)
        {
            // Verificamos que vidaInicial no sea cero para evitar divisiones por cero
            if (vidaInicial > 0)
            {
                // Calcula el porcentaje usando la vida actual y la vida inicial
                RellenobarraVida.fillAmount = jugador.vida / vidaInicial;
            }
        }
    }
}