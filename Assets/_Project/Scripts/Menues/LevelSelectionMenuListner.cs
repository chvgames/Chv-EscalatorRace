using System;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIEffects;

public class LevelSelectionMenuListner : MonoBehaviour {

	//public Transform scrollContent;
	public LevelSelectionLevelBtnHandler[] buttons;

	//public Text goldTxt;

	public GameObject buyAllLevelsBtn;

	public RectTransform content;
	public Sprite selectedBtnImage;

	//private void OnEnable()
	//{
	//	UpdateTxt();
	//	PurchaseCheck();

	//}
	void Start()
	{
		SetLevelsButtonLockState();
		Highlight_CurrBtn();

		int mul = Mathf.RoundToInt(Toolbox.DB.prefs.LastSelectedLevel / 2);
		content.transform.position = new Vector3(-400 * mul, 0, 0) ;

	}
	//void UpdateTxt()
	//{
	//	goldTxt.text = Toolbox.DB.prefs.GoldCoins.ToString();
	//}

	public void OnPress_Shop()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		Toolbox.GameManager.Instantiate_Shop();
	}

	private void Highlight_CurrBtn()
    {
		UIShiny _object = buttons[Toolbox.DB.prefs.LastSelectedLevel].gameObject.AddComponent<UIShiny>();
		_object.effectPlayer.loop = true;
		_object.effectPlayer.duration = 2.7f;
		_object.effectPlayer.play = true;

		buttons[Toolbox.DB.prefs.LastSelectedLevel].btnImg.sprite = selectedBtnImage;
	}
	public void SetLevelsButtonLockState()
    {
		for (int i = 0; i < buttons.Length; i++)
		{
			buttons[i].SetButtonStats(i);
		}
	}

    void Update(){

		if (Input.GetKeyDown (KeyCode.Escape)) {

			Press_BackButton ();
		}
	}

	public void OnPress_LevelButton(int _val){ // this will instantiate next menu in arraylist

		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.DB.prefs.LastSelectedLevel = _val;
		Toolbox.MenuHandler.Show_NextUI();        
    }

	public void Press_BackButton(){

		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.back);
		Toolbox.MenuHandler.Show_PrevUI();
	}

	public void OnPress_UnlockAllLevels()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
		//Toolbox.InAppHandler.BuyProductID(Constants.unlockLevels);
	}

	public void PurchaseCheck()
	{

		if (Toolbox.DB.prefs.AllLevelsUnlockedPurchased)
			buyAllLevelsBtn.SetActive(false);
	}

}
