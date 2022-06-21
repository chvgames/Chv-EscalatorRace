using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    public Transform playerSpawnPoint;

    public GameObject endAnimationObj;

    public Transform completeCarSpawnPoint;
    
    public void LevelStartHandling() {

        Toolbox.GameManager.Instantiate_Blackout();
        Toolbox.HUDListner.EnableHUD();
        Toolbox.GameplayScript.camListner.GetComponent<Camera>().enabled = true;
    }

    public void LevelEndHandling() {

        Toolbox.GameManager.Instantiate_Blackout();
        Toolbox.GameplayScript.camListner.GetComponent<Camera>().enabled = false;

        endAnimationObj.SetActive(true);

    }
}
