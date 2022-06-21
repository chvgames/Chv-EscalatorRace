using UnityEngine;


[CreateAssetMenu(fileName = "LevelData", menuName = ("Level Data"))]
public class LevelData : ScriptableObject
{
    [Range(1,5)]
    public int levelGameScene = 2;
    [Range(0, 30)]
    public int carNumber = 0;
    [Range(0, 2)]
    public int environmentNumber = 0;
    [Space(10)]
    public int allowedPanelties = 5;
    public bool ShowTutorial = false;
    public int playerObjInStart = 5;

    public enum Weather { 
    
        SUNNY,
        RAINY,
        SNOWY
    }
    public Weather weather = Weather.SUNNY;

    public enum TrafficType
    {
        NONE,
        BOTH,
        UPWARD,
        DOWNWARD
    }
    public TrafficType traffic = TrafficType.BOTH;

    public bool isNight = false;
    public bool hasExtras = false;

    [SerializeField] public AudioClip bgSound;

    [Space(20)]
    public EnvProfile envProfile;
}
