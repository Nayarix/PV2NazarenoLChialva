using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    public static ProgressionManager Instance;

    public ProgressionData progressionData;
    public Jugador jugador;

    private int currentLevel;
    private float currentExp;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        currentLevel = 0;
        currentExp = 0;
    }

    public void AddExperience(float expToAdd)
    {
        currentExp += expToAdd;
        Debug.Log("Experiencia actual: " + currentExp);

        while (currentLevel < progressionData.levels.Length && currentExp >= progressionData.levels[currentLevel].expRequired)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        
        currentLevel++;
        Debug.Log("¡Subiste de nivel! Nivel actual: " + currentLevel);

        
        jugador.ModificarVida(progressionData.levels[currentLevel - 1].healthIncrease);
        jugador.ModificarDanio(progressionData.levels[currentLevel - 1].damageIncrease);
    }
}