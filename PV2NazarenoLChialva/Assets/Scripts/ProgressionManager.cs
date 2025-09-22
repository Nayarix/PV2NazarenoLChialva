using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
    
    public static ProgressionManager Instance;

    public ProgressionData progressionData;
    public Jugador jugador;

    private int currentLevel;
    private float currentExp;
    private float expToNextLevel;

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
        expToNextLevel = progressionData.levels[0].expRequired;
    }

    public void AddExperience(float expToAdd)
    {
        currentExp += expToAdd;
        Debug.Log("Experiencia actual: " + currentExp);

        if (currentExp >= expToNextLevel)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        if (currentLevel < progressionData.levels.Length)
        {
            currentLevel++;
            Debug.Log("¡Subiste de nivel! Nivel actual: " + currentLevel);

            
            jugador.ModificarVida(progressionData.levels[currentLevel - 1].healthIncrease);
            jugador.ModificarDanio(progressionData.levels[currentLevel - 1].damageIncrease);

            
            if (currentLevel < progressionData.levels.Length)
            {
                expToNextLevel = progressionData.levels[currentLevel - 1].expRequired;
            }
            else
            {
                expToNextLevel = Mathf.Infinity; 
            }
        }
    }
}