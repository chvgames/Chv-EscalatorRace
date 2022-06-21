using System;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectionListner : MonoBehaviour
{
	public Text goldTxt;

	public GameObject [] modesLockObject;
	public Text[] levelValueTxt;


    private void Start()
    {
		HandleModeLock();
		UpdateCoins();
		UpdateTxts();
	}

    private void HandleModeLock() {

        for (int i = 0; i < Toolbox.DB.prefs.ModeUnlocked.Length; i++)
        {
			if (Toolbox.DB.prefs.ModeUnlocked[i])
				modesLockObject[i].SetActive(false);
		}

	}

    void UpdateCoins()
	{
		goldTxt.text = Toolbox.DB.prefs.GoldCoins.ToString();
	}

	private void UpdateTxts()
	{
		for (int i = 0; i < levelValueTxt.Length; i++)
		{
			if (Toolbox.DB.prefs.ModeUnlocked[i])
				levelValueTxt[i].text = (Toolbox.DB.prefs.GameMode[i].GetLastUnlockedLevel() + 1) + "/" + Constants.maxLevelsOfMode[i];
		}
	}

	public void OnPress_Back()
	{
			Toolbox.MenuHandler.Show_PrevUI();
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.back);
	}

	public void OnPress_Mode(int _val)
	{
		Toolbox.DB.prefs.LastSelectedMode = _val;
		Toolbox.DB.prefs.LastSelectedLevel = Toolbox.DB.prefs.GameMode[_val].GetLastUnlockedLevel();
		
		Toolbox.MenuHandler.Show_NextUI();
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
	}

	public void OnPress_ModeLock(int _val) {

		Toolbox.GameManager.InstantiatePopup_Message("This mode is Locked. It will be available soon.");
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Select);
	}
}
