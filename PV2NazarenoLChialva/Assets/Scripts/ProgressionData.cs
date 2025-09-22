using UnityEngine;

[CreateAssetMenu(fileName = "ProgressionData", menuName = "Progression/ProgressionData")]
public class ProgressionData : ScriptableObject
{
   
    [System.Serializable]
    public class LevelData
    {
        public int level;
        public float expRequired;
        public float healthIncrease;
        public float damageIncrease;
    }

    public LevelData[] levels;
}