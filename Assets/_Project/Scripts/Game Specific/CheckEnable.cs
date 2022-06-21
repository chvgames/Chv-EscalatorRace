using UnityEngine;

public class CheckEnable : MonoBehaviour
{
    public GameObject[] checkThings;

    private void OnEnable()
    {
        for (int i = 0; i < checkThings.Length; i++)
        {
            if (checkThings[i].activeSelf) {

                this.gameObject.SetActive(false);
                break;
            }
        }
    }
}
