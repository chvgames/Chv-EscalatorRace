using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PowerupShop : MonoBehaviour
{
    [Header("TMP")]
    [SerializeField] private Text coinsStack;
    [SerializeField] private TextMeshProUGUI divideStack;
    [SerializeField] private TextMeshProUGUI slowStack;
    [SerializeField] private TextMeshProUGUI speedStack;
    [SerializeField] private TextMeshProUGUI shieldStack;

    private void OnEnable()
    {
        coinsStack.text = Toolbox.DB.prefs.GoldCoins.ToString();
        divideStack.text = Toolbox.DB.prefs.DivideImmunityStack.ToString();
        slowStack.text = Toolbox.DB.prefs.SlowStack.ToString();
        speedStack.text = Toolbox.DB.prefs.SpeedStack.ToString();
        shieldStack.text = Toolbox.DB.prefs.ShieldStack.ToString();
    }

    public void BuyDivide(int price)
    {
        if (price > Toolbox.DB.prefs.GoldCoins)
        {
            Toolbox.GameManager.InstantiatePopup_Message1("You don't have enough coin");
            return;
        }

        Toolbox.GameplayScript.DeductGoldCoins(price);
        Toolbox.DB.prefs.DivideImmunityStack++;
        divideStack.text = Toolbox.DB.prefs.DivideImmunityStack.ToString();
        coinsStack.text = Toolbox.DB.prefs.GoldCoins.ToString();
    }
    public void BuySlow(int price)
    {
        if (price > Toolbox.DB.prefs.GoldCoins)
        {
            Toolbox.GameManager.InstantiatePopup_Message1("You don't have enough coin");
            return;
        }

        Toolbox.GameplayScript.DeductGoldCoins(price);
        Toolbox.DB.prefs.SlowStack++;
        slowStack.text = Toolbox.DB.prefs.SlowStack.ToString();
        coinsStack.text = Toolbox.DB.prefs.GoldCoins.ToString();
    }
    public void BuySpeed(int price)
    {
        if (price > Toolbox.DB.prefs.GoldCoins)
        {
            Toolbox.GameManager.InstantiatePopup_Message1("You don't have enough coin");
            return;
        }

        Toolbox.GameplayScript.DeductGoldCoins(price);
        Toolbox.DB.prefs.SpeedStack++;
        speedStack.text = Toolbox.DB.prefs.SpeedStack.ToString();
        coinsStack.text = Toolbox.DB.prefs.GoldCoins.ToString();
    }
    public void BuyShield(int price)
    {
        if (price > Toolbox.DB.prefs.GoldCoins)
        {
            Toolbox.GameManager.InstantiatePopup_Message1("You don't have enough coin");
            return;
        }

        Toolbox.GameplayScript.DeductGoldCoins(price);
        Toolbox.DB.prefs.ShieldStack++;
        shieldStack.text = Toolbox.DB.prefs.ShieldStack.ToString();
        coinsStack.text = Toolbox.DB.prefs.GoldCoins.ToString();
    }
    public void OnPress_Back()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);
        Destroy(this.gameObject);
    }
}
