using UnityEngine;
using UnityEngine.UI;

public class MainMenuListner : MonoBehaviour {

	public Text goldTxt;
	public Text lvlTxt;
	public GameObject noAdsBtn;
	public GameObject restoreBtn;
    private void OnEnable()
    {
		UpdateTxt();
		PurchaseCheck();
        #if UNITY_ANDROID
       	restoreBtn.SetActive(false);
        #endif


	}
    private void Start()
    {
        if (Toolbox.GameManager.showSkin)
        {
			Toolbox.GameManager.showSkin = false;
			OnPress_StoreSkin();

		}
    }

    public void PurchaseCheck() {

		if (Toolbox.DB.prefs.NoAdsPurchased)
			noAdsBtn.SetActive(false);
	}

    public void UpdateTxt(){

		lvlTxt.text = "Level " + (Toolbox.DB.prefs.LastSelectedLevel + 1).ToString();
		goldTxt.text = Toolbox.DB.prefs.GoldCoins.ToString();

	}

    public void OnPress_Settings()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		Toolbox.GameManager.Instantiate_SettingsMenu();	
	}
	public void OnPress_Store()
    {
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		Toolbox.GameManager.Instantiate_Shop();
	}

	public void OnPress_Quests()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		Toolbox.GameManager.Instantiate_Quest();
	}

	public void OnPress_Fb()
	{
		Application.OpenURL(Constants.fb);
	}

	public void OnPress_RateUs()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		Application.OpenURL(Constants.appLink);
	}
	public void OnPress_RemoveAds()
	{
		IAPManager.instance.BuyRemoveAds();
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
	}
	public void OnPress_DailyReward()
    {
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		Toolbox.GameManager.Instantiate_DailyRewardMenu();

	}
	public void OnPress_MoreGames()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		Application.OpenURL(Constants.moreGamesLink);
	}

	public void OnPress_Back()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);
		Toolbox.MenuHandler.Show_PrevUI();
	}
	public void OnPress_Play()
    {
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.GameManager.LoadScene(Constants.sceneIndex_Map, true, 0);
	}

    public void OnPress_Next()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.MenuHandler.Show_NextUI();
	}
	public void OnPress_StoreSkin()
    {
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.GameManager.Instantiate_StoreSkin();
	}
	public void OnPress_PowerupsSkin()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.GameManager.Instantiate_Powerups();
	}
	public void OnPress_StoreCars()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.GameManager.Instantiate_StoreCars();
	}
	public void OnPress_RestorePurchase()
	{
		IAPManager.instance.RestorePurchases();
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
	}
	

}
