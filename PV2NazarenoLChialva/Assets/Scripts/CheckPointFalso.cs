using UnityEngine;

public class CheckpointFalso : MonoBehaviour
{
    public GeneradorObjeto[] generadoresParaActivar;

    private bool yaActivado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !yaActivado)
        {
            ActivarGeneradores();
            yaActivado = true;

            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void ActivarGeneradores()
    {
        if (generadoresParaActivar.Length == 0)
        {
            Debug.LogWarning("El Checkpoint Falso no tiene ningún Generador asignado.");
            return;
        }

        foreach (GeneradorObjeto generador in generadoresParaActivar)
        {
            if (generador != null)
            {
                generador.IniciarGeneracionEnemigos();
            }
        }
    }
}