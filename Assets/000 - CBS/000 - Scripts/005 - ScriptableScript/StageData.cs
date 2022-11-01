using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StageData", menuName = "CBS/Stage/StageData")]
public class StageData : ScriptableObject
{
    public enum Stage
    {
        NONE,
        CITY,
        SNOW,
        DESERT
    }
    [field: SerializeField] public Stage CurrentStage { get; set; }
    [field: SerializeField] public int currentStageNumber { get; set; }
    [field: SerializeField] public bool isBossFight { get; set; }
}
