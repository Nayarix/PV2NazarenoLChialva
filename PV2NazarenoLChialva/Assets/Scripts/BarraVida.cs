using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image RellenobarraVida;
    
    public Jugador jugador;
    private float vidaInicial; 

    void Start()
    {
        if (jugador != null)
        {
            vidaInicial = jugador.vidaActual; 
        }
    }

    void Update()
    {
        
        if (jugador != null)
        {
           
            if (vidaInicial > 0)
            {
                
                RellenobarraVida.fillAmount = jugador.vidaActual / vidaInicial;
            }
        }
    }
}