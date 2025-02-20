﻿using System;
using UnityEngine;
using UnityEngine.Purchasing;


public class IAPManager : MonoBehaviour, IStoreListener
{
    public static IAPManager instance;

    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;


    private string coinsPack1 = "custom_coins_pack1";
    private string coinsPack2 = "custom_coins_pack2";
#if UNITY_IPHONE
        private string removeAds = "No_Ads_Esclator";
#elif UNITY_ANDROID
    private string removeAds = "esc_no_ads";

        #endif



    //************************** Adjust these methods **************************************
    public void InitializePurchasing()
    {
        if (IsInitialized()) { return; }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        //Step 2 choose if your product is a consumable or non consumable
        builder.AddProduct(removeAds, ProductType.NonConsumable);
        builder.AddProduct(coinsPack1, ProductType.NonConsumable);
        builder.AddProduct(coinsPack2, ProductType.NonConsumable);

        UnityPurchasing.Initialize(this, builder);
    }


    private bool IsInitialized()
    {
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }


    //Step 3 Create methods
    public string GetPrice1()
    {
        if (m_StoreController == null) InitializePurchasing();
        return m_StoreController.products.WithID(coinsPack1).metadata.localizedPriceString;
    }
    public string GetPrice2()
    {
        if (m_StoreController == null) InitializePurchasing();
        return m_StoreController.products.WithID(coinsPack2).metadata.localizedPriceString;
    }
    public void BuyRemoveAds()
    {
        BuyProductID(removeAds);
    }
    public void BuyCoinPack1()
    {
        BuyProductID(coinsPack1); 
    }
    public void BuyCoinsPack2()
    {
        BuyProductID(coinsPack2);
    }


    //Step 4 modify purchasing
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {
        if (String.Equals(args.purchasedProduct.definition.id, removeAds , StringComparison.Ordinal))
        {
            Toolbox.DB.prefs.NoAdsPurchased = true;

            FindObjectOfType<MainMenuListner>().noAdsBtn.SetActive(false);
            Debug.Log("Remove Ads Successfully");
        }
        if (String.Equals(args.purchasedProduct.definition.id, coinsPack1, StringComparison.Ordinal))

        {
            PlayerPrefs.SetInt("CoinsPack1", 1);
            Toolbox.GameManager.InstantiatePopup_Message("You have purchased successfully");
            Toolbox.GameplayScript.IncrementGoldCoins(30000);
            FindObjectOfType<ShopListner>().goldTxt.text = Toolbox.DB.prefs.GoldCoins.ToString();
        }
        if (String.Equals(args.purchasedProduct.definition.id, coinsPack2, StringComparison.Ordinal))

        {
            PlayerPrefs.SetInt("CoinsPack2", 1);

            Toolbox.GameManager.InstantiatePopup_Message("You have purchased successfully");
            Toolbox.GameplayScript.IncrementGoldCoins(75000);
            FindObjectOfType<ShopListner>().goldTxt.text = Toolbox.DB.prefs.GoldCoins.ToString();
        }
        else
        {
            Debug.Log("Purchase Failed");
        }
        return PurchaseProcessingResult.Complete;
    }










    //**************************** Dont worry about these methods ***********************************
    private void Awake()
    {
        TestSingleton();
    }

    void Start()
    {
        if (m_StoreController == null) { InitializePurchasing(); }
    }

    private void TestSingleton()
    {
        if (instance != null) { Destroy(gameObject); return; }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void BuyProductID(string productId)
    {
        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product); 
            }
            else
            {
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
        else
        {
            Debug.Log("BuyProductID FAIL. Not initialized.");
        }
    }

    public void RestorePurchases()
    {
        if (!IsInitialized())
        {
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        if (Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
            Debug.Log("RestorePurchases started ...");

            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            apple.RestoreTransactions((result) => {
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        else
        {
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized: PASS");
        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }


    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
    }
    
}