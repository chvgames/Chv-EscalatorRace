using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// 
/// Every Menu will normally have:
/// Farward button : to next screen
/// Backward button : to previous screen
/// 
/// </summary>

public class MenuHandler : MonoBehaviour {


	int currentMenuNum = 0;

//	public GameObject clone;
	public GameObject[] menuList;

	private LevelData curLevelData;
	private DateTime lastRewardTime;

	void Awake(){

		Toolbox.Set_MenuHandler(this);
		Toolbox.Soundmanager.PlayBGSound(Toolbox.Soundmanager.menuBG);
        //AdsManager.instance.RequestAd(AdsManager.AdType.INTERSTITIAL);

	}

    private void Start()
    {
		//lastRewardTime = Toolbox.DB.prefs.LastClaimedRewardTime;
		//if(DateTime.Now >= lastRewardTime.AddHours(24)) 
		//	Toolbox.GameManager.Instantiate_DailyRewardMenu(); 
		StartMainMenu();
	}


	void StartMainMenu()
	{
		if (menuList[0])
		{
			if (Toolbox.GameManager.directShowVehicleSel)
			{
				Toolbox.GameManager.directShowVehicleSel = false;
				menuList[1].SetActive(true);

				currentMenuNum = 1;
			}
			else
			{
				menuList[0].SetActive(true);
				currentMenuNum = 0;
			}
		}
		else
		{
			Debug.LogError("MainMenu is not initialized in MenuHandler");
		}

	}

	public void Show_NextUI(){

		if (currentMenuNum + 1 < menuList.Length) {

			menuList[currentMenuNum].SetActive(false);
			menuList[currentMenuNum + 1].SetActive(true);

			currentMenuNum++;
					
		} else {

			Debug.Log ("Menu List has reached at last menu. Loading GameScene!");

			Toolbox.GameManager.Log("MYLOG - MENU END");

			Toolbox.Soundmanager.Stop_PlayingBGSound();

			AdsManager.instance.ShowAd(AdsManager.AdType.INTERSTITIAL);

			Toolbox.GameManager.LoadScene(Toolbox.GameManager.GetCurrentLevelGameScene(), true, 0);
        }

    }

	//public void LoadGameplayScene()
 //   {
	//	//string path = Constants.PrefabFolderPath + Constants.LevelsScriptablesFolderPath + Toolbox.DB.prefs.LastSelectedLevel.ToString();
		
	//	//curLevelData = (LevelData)Resources.Load(path);

	//	Toolbox.GameManager.LoadScene(Toolbox.GameManager.GetCurrentLevelGameScene(), true, 0);

	//}

	public void Show_PrevUI(){

		if (currentMenuNum - 1 >= 0) {

			menuList[currentMenuNum].SetActive(false);
			menuList[currentMenuNum - 1].SetActive(true);

			currentMenuNum--;

		} else {

			Debug.Log ("Menu List has reached at First menu. This is the limit!");
		}

	}

}
