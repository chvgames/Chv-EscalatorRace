using UnityEngine;
using UnityEngine.UI;

public class PlayerObjSelectionListner : MonoBehaviour
{
	public Text goldTxt;

	public int curIndex = 0;
	public int curPaintIndex = 0;

	public GameObject environmentObj;
	public Transform objSpawnPoint;
	public Transform rotateObj;

	public GameObject spawnedObj;
	private PlayerObjData spawnedPlayerData;

	public GameObject normalView;
	public GameObject colorView;

	public Text upgradeBtnLevelTxt;

	public GameObject playBtn;
	public GameObject unlockBtn;
	public Button upgradeBtn;
	public GameObject colorBtn;
	public float RotationSpeed;

	public GameObject buyAllPlayerObjBtn;

	[Header("Specs")]
	public Text nameTxt;
	public Text priceCostTxt;
	public GameObject priceObj;
	public Image[] specs;
	public ImageFillInStepsHandler[] specsFillInPoint;

	[Header("Paint")]
	public Button paintApplyBtn;
	public Text paintCostTxt;
	public GameObject priceObject;
	public Image[] paintImages;


	private float xAxix;
	private void OnEnable()
	{
		UpdateTxt();
		curIndex = Toolbox.DB.prefs.LastSelectedPlayerObj;
		SpawnObject(curIndex);

		environmentObj.SetActive(true);
		PurchaseCheck();
		ShowView(true);
	}

	private void OnDisable()
	{
		if (environmentObj)
			environmentObj.SetActive(false);
	}

	public void UpdateValues() {
		UpdateTxt();
		UpdateUI();
	}

	public void UpdateTxt()
	{
		goldTxt.text = Toolbox.DB.prefs.GoldCoins.ToString();
	}

	private void UpdateUI()
	{
		if (Toolbox.DB.prefs.PlayerObjectBought[curIndex])
		{
			unlockBtn.SetActive(false);
			priceObj.gameObject.SetActive(false);
			playBtn.SetActive(true);
			upgradeBtn.gameObject.SetActive(true);
			colorBtn.gameObject.SetActive(true);
		}
		else
		{

			playBtn.SetActive(false);
			unlockBtn.SetActive(true);
			priceObj.gameObject.SetActive(true);
			upgradeBtn.gameObject.SetActive(false);
			colorBtn.gameObject.SetActive(false);
		}

		nameTxt.text = spawnedPlayerData.name;

		priceCostTxt.text = spawnedPlayerData.price.ToString();
		paintCostTxt.text = Constants.playerPaintCost.ToString();

		//for (int i = 0; i < specs.Length; i++)
		//{
		//	specs[i].fillAmount = spawnedPlayerData.specs[i];
		//	specsFillInPoint[i].FillImage(specs[i].fillAmount);
		//}

		int curUpgradeLevel = Toolbox.DB.prefs.PlayerObjectUpgradeLvl[curIndex];

		for (int i = 0; i < specs.Length; i++)
		{
			float val = spawnedPlayerData.upgradeLvl[curUpgradeLevel].specs[i].specValue / Constants.maxSpecValue[i];

			specs[i].fillAmount = val;
			specsFillInPoint[i].FillImage(val);
		}

		upgradeBtnLevelTxt.text = (curUpgradeLevel + 1) + "/" + (Constants.maxPlayerUpgradeLevel + 1);

		if (curUpgradeLevel >= Constants.maxPlayerUpgradeLevel)
			upgradeBtn.interactable = false;
		else
			upgradeBtn.interactable = true;


	}

	public void Update()
	{
		//xAxix = CnInputManager.GetAxis("Horizontal");

		//rotateObj.transform.Rotate(Vector3.up, xAxix * Time.deltaTime * RotationSpeed);
	}

	public void ShowView(bool _normalView) {

		if (_normalView == true)
		{
			normalView.SetActive(true);
			colorView.SetActive(false);
		}
		else {
			normalView.SetActive(false);
			colorView.SetActive(true);
		}
	}

	public void PurchaseCheck()
	{
		if (Toolbox.DB.prefs.AllPlayerObjPurchased)
			buyAllPlayerObjBtn.SetActive(false);
	}

	private void SpawnObject(int _val)
	{
		string path = Constants.PrefabFolderPath + Constants.PlayerFolderPath + _val.ToString();
		//Toolbox.GameManager.Log("Vehicle path = " + path);

		if (spawnedObj)
			Destroy(spawnedObj);

		spawnedObj = (GameObject)Instantiate(Resources.Load(path), objSpawnPoint.position, objSpawnPoint.rotation, objSpawnPoint);

		path = Constants.PrefabFolderPath + Constants.PlayerScriptablesFolderPath + _val.ToString();

		spawnedPlayerData = (PlayerObjData)Resources.Load(path);
		SetPlayerObjColor(Toolbox.DB.prefs.PlayerObjectPaintValue[curIndex]);

		UpdateUI();
	}

	public void OnPress_Prev()
	{
		if (curIndex - 1 < 0) {

			curIndex = Constants.maxPlayerObjects+1;
		}

		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.rightLeftObj);
		curIndex--;
		SpawnObject(curIndex);
	}

	public void OnPress_Next()
	{
		if (curIndex >= Constants.maxPlayerObjects) {

			curIndex = -1;
		}

		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.rightLeftObj);
		curIndex++;
		SpawnObject(curIndex);
	}

	public void OnPress_Shop()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		Toolbox.GameManager.Instantiate_Shop();
	}

	public void OnPress_Setting()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		Toolbox.GameManager.Instantiate_SettingsMenu();
	}
	public void OnPress_Back()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);
		Destroy(this.gameObject);
	}

	public void OnPress_Play()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);

		if (Toolbox.DB.prefs.PlayerObjectBought[curIndex])
			Toolbox.DB.prefs.LastSelectedPlayerObj = curIndex;

		AdsManager.instance.ShowAd(AdsManager.AdType.INTERSTITIAL);
		Toolbox.MenuHandler.Show_NextUI();
	}

	public void OnPress_Unlock()
	{

		if (Toolbox.DB.prefs.GoldCoins >= spawnedPlayerData.price)
		{

			Toolbox.DB.prefs.GoldCoins -= spawnedPlayerData.price;
			Toolbox.DB.prefs.PlayerObjectBought[curIndex] = true;

			Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.vehicleUnlock);
			Toolbox.GameManager.InstantiatePopup_Message("Congratulations! Vehicle Unlocked.");
		}
		else {
			Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
			Toolbox.GameManager.InstantiatePopup_Message("You don't have enough money.");
		}

		UpdateUI();
		UpdateTxt();
	}



	public void Buy_AllCars() {

		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		//Toolbox.InAppHandler.BuyProductID(Constants.unlockPlayerObj);
	}

	public void Press_UpgradeBtn()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.GameManager.Instantiate_UpgradeMenu(spawnedPlayerData, curIndex);
	}

	public void Press_ColorBtn()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		ShowView(false);
	}

	//Paint Functions

	public void PressPaintBack() {

		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.back);
		ShowView(true);
	}

	public void SetPlayerObjColor(int _index)
	{
		paintImages[curPaintIndex].transform.GetChild(0).gameObject.SetActive(false);

		curPaintIndex = _index;

		paintImages[curPaintIndex].transform.GetChild(0).gameObject.SetActive(true);

		if (_index == Toolbox.DB.prefs.PlayerObjectPaintValue[curIndex])
		{
			paintApplyBtn.interactable = false;
			priceObject.SetActive(false);
		}
		else {
			paintApplyBtn.interactable = true;
			priceObject.SetActive(true);
		}

		if(Toolbox.DB.prefs.GoldCoins < Constants.playerPaintCost)
			paintApplyBtn.interactable = false;

	}

	public void PaintUpgrade() {

		Toolbox.DB.prefs.GoldCoins -= Constants.playerPaintCost;
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.paint);

		Toolbox.DB.prefs.PlayerObjectPaintValue[curIndex] = curPaintIndex;
		ShowView(true);

		UpdateValues();
	}
}
