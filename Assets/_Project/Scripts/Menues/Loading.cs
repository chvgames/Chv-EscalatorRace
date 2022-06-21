using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour {

	public Image loadingBar;
	
	[SerializeField]
	private bool isTemporaryLoading = false;

    public float delayTime = 2;

    public bool IsTemporaryLoading { get => isTemporaryLoading; set => isTemporaryLoading = value; }

    private void OnEnable()
    {
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Loading);

		//AdsManager.instance.RequestBannerWithSpecs( Tapdaq.TDMBannerSize.TDMBannerLarge,  Tapdaq.TDBannerPosition.Top);
	}

    private void OnDestroy()
    {
        //AdsManager.instance.HideBannerAd();
    }

    private void Start()
    {
		if (isTemporaryLoading) {

			DontDestroyOnLoad(this.gameObject);
		}
    }

    void Update () {

		if (loadingBar && Toolbox.GameManager.async != null) {
		
			loadingBar.fillAmount = Toolbox.GameManager.async.progress;
		}

		if (isTemporaryLoading) {

			delayTime -= Time.deltaTime;

			if (delayTime <= 0) {

				Toolbox.GameManager.Log("MYLOG - In Zero Time");
				isTemporaryLoading = false;

				if (SceneManager.GetActiveScene().buildIndex == Toolbox.GameManager.nextSceneIndex) {
					Toolbox.GameManager.Log("MYLOG - In Build Index");

					//if (Toolbox.GameplayScript)
					//	Toolbox.GameplayScript.levelsManager.StartLevelHandling();

					Toolbox.GameManager.Log("MYLOG - Destroying Loading");
					Destroy(this.gameObject);
				}

			}
		}
	}

}
