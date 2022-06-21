using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenuListner : MonoBehaviour
{
    public Text upgradePriceTxt;
    public Button upgradeBtn;
    public PlayerObjData spawnedPlayerData;
    public int curPlayerIndex = 0;

    public ImageFillInStepsHandler [] currentLevelFill;
    public ImageFillInStepsHandler[] upgradeLevelFill;

    public void UpdateValues()
    {
        int curUpgradeLevel = Toolbox.DB.prefs.PlayerObjectUpgradeLvl[curPlayerIndex];

        for (int i = 0; i < currentLevelFill.Length; i++)
        {
            float curVal = spawnedPlayerData.upgradeLvl[curUpgradeLevel].specs[i].specValue / Constants.maxSpecValue[i];
            float upgradeVal = spawnedPlayerData.upgradeLvl[curUpgradeLevel+1].specs[i].specValue / Constants.maxSpecValue[i];

            currentLevelFill[i].FillImage(curVal);
            upgradeLevelFill[i].FillImage(upgradeVal);
        }

        upgradePriceTxt.text = Constants.playerUpgradeCost.ToString();

        if (Toolbox.DB.prefs.GoldCoins < Constants.playerUpgradeCost || curUpgradeLevel >= Constants.maxPlayerUpgradeLevel)
        {
            upgradeBtn.interactable = false;
        }
    }

    public void Press_Upgrade()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.vehicleUpgrade);

        Toolbox.DB.prefs.GoldCoins -= Constants.playerUpgradeCost;
        
        Toolbox.DB.prefs.PlayerObjectUpgradeLvl[curPlayerIndex]++;

        Toolbox.MenuHandler.GetComponentInChildren<PlayerObjSelectionListner>().UpdateValues();

        Destroy(this.gameObject);
    }

    public void Press_Close() {

        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.back);

        Destroy(this.gameObject);
    }
}
