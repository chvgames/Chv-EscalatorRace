using UnityEngine;

public class PauseListner : MonoBehaviour {

	void OnDestroy(){
		Toolbox.Soundmanager.UnPause_All ();
		Time.timeScale = 1;
		AudioListener.pause = false;
	}
    private void OnEnable()
    {
		Toolbox.Soundmanager.Pause_All();
		AudioListener.pause = true;
	}
    private void Start()
    {
		Time.timeScale = 0;
	}

    void Update(){

		if (Input.GetKeyDown (KeyCode.Escape)) {

			Press_Play ();
		}
	}

	public void Press_Play(){
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.HUDListner.EnableHUD();


		Destroy(this.gameObject);
	}
	
	public void Press_Restart()
	{
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.GameManager.LoadScene(Toolbox.GameManager.GetCurrentLevelGameScene(), true, 0);
		AdsManager.instance.ShowAd(AdsManager.AdType.INTERSTITIAL);

		Destroy(this.gameObject);
	}

	public void Press_Home(){

		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressYes);
		Toolbox.GameManager.LoadScene(Constants.sceneIndex_Menu, true, 0);
		AdsManager.instance.ShowAd(AdsManager.AdType.INTERSTITIAL);

		Destroy(this.gameObject);
	}

}
