using UnityEngine;
using UnityEngine.UI;

public class ShopListner : MonoBehaviour
{
    public Text goldTxt;
    public Text coin1Txt;
    public Text coin2Txt;
    
    private void OnEnable()
    {
        UpdateTxt();
    }

    void UpdateTxt()
    {
        goldTxt.text = Toolbox.DB.prefs.GoldCoins.ToString();


        coin1Txt.text = IAPManager.instance.GetPrice1();
        coin2Txt.text = IAPManager.instance.GetPrice2();
    }



    public void OnPress_Close()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);
        Destroy(this.gameObject);
    }

    public void OnPress_FreeCoins()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);
        AdsManager.instance.SetNShowRewardedAd(AdsManager.RewardType.FREECOINS, 100);
    }

    public void PurchaseProduct(int _val)
    {

        switch (_val) {


            case 0:
                IAPManager.instance.BuyCoinPack1();
                Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
                //Toolbox.InAppHandler.BuyProductID(Constants.coins_1);


                break;

            case 1:
                IAPManager.instance.BuyCoinsPack2();
                Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
                //Toolbox.InAppHandler.BuyProductID(Constants.coins_2);
                break;

            case 2:

                //Toolbox.InAppHandler.BuyProductID(Constants.unlockPlayerObj);
                break;

            case 3:

                //Toolbox.InAppHandler.BuyProductID(Constants.unlockLevels);
                break;
            case 4:

                //Toolbox.InAppHandler.BuyProductID(Constants.unlockLevels);
                break;

        }

    }
}
