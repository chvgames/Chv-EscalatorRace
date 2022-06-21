using UnityEngine;

public class FinishLister : MonoBehaviour
{
    public Transform[] areaStandPoints;

    public int curStandPointIndex = 0;
    public bool maxFilled = false;
    public Transform childParent;

    public void AddInArea(CharacterHandler _controller)
    {
        _controller.transform.parent = this.childParent;

        if (maxFilled)
        {
            _controller.autoMove = false;
            _controller.transform.position = areaStandPoints[curStandPointIndex].position;
            _controller.transform.rotation = areaStandPoints[curStandPointIndex].rotation;
        }
        else
        {

            _controller.SetAutoMove(areaStandPoints[curStandPointIndex]);
        }

        curStandPointIndex++;

        if (curStandPointIndex >= areaStandPoints.Length)
        {
            maxFilled = true;
            curStandPointIndex = 0;
        }
    }

    public void DisableAllCharacters() {

        for (int i = 0; i < childParent.childCount; i++)
        {
            childParent.GetChild(i).gameObject.SetActive(false);
        }
    }
}
