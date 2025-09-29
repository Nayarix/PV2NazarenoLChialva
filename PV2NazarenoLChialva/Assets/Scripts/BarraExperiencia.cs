using UnityEngine;
using UnityEngine.UI;

public class BarraExperiencia : MonoBehaviour
{
    [Header("Componentes UI")]
    public Image RellenoBarraExp;

    [Header("Referencias del Sistema")]
    private ProgressionManager progressionManager;

    void Start()
    {
        progressionManager = ProgressionManager.Instance;

        if (progressionManager == null)
        {
            Debug.LogError("El script BarraExperiencia no encontró la instancia de ProgressionManager.");
        }

        ActualizarBarra();
    }

    void Update()
    {
        ActualizarBarra();
    }

    public void ActualizarBarra()
    {
        if (progressionManager == null || RellenoBarraExp == null) return;

        int nivelActual = progressionManager.GetCurrentLevel();
        float experienciaActual = progressionManager.GetCurrentExp();


        float expRequeridaAnterior = 0f;

        if (nivelActual > 0)
        {
            expRequeridaAnterior = progressionManager.GetRequiredExpForLevel(nivelActual - 1);
        }

        float expRequeridaSiguiente = progressionManager.GetRequiredExpForLevel(nivelActual);


        float expNetaActual = experienciaActual - expRequeridaAnterior;

        float rangoExpNivel = expRequeridaSiguiente - expRequeridaAnterior;

        if (rangoExpNivel > 0)
        {
            RellenoBarraExp.fillAmount = expNetaActual / rangoExpNivel;
        }
        else
        {
            RellenoBarraExp.fillAmount = 1f;
        }
    }
}