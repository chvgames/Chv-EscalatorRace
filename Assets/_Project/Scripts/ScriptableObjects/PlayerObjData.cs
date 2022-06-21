using UnityEngine;

[System.Serializable]
public class UpgradeSpec
{
    public string name;
    public float specValue;
}

[System.Serializable]
public class VehicleUpgrades
{
    public UpgradeSpec[] specs;
}


[CreateAssetMenu(fileName = "PlayerObjData", menuName = ("Player Data"))]
public class PlayerObjData : ScriptableObject
{
    public string name;
    public int price;

    [Range(0,1)]
    public float [] specs;

    public VehicleUpgrades[] upgradeLvl;
}
