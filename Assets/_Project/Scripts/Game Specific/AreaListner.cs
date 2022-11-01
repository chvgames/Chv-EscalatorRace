using UnityEngine;

public class AreaListner : MonoBehaviour
{
    public bool isBossArea = false;
    public Transform camPosition;

    public Transform[] areaStandPoints;

    int areaIndex = 0;
    public int inArea = 0;
    public int curStandPointIndex = 0;
    public bool maxFilled = false;

    public float expansionVal = 0.2f;

    public void AddInArea(CharacterHandler _controller) {

        inArea++;

;
        if (maxFilled)
        {
            _controller.autoMove = false;
            _controller.transform.position = areaStandPoints[curStandPointIndex].position;
            _controller.transform.rotation = areaStandPoints[curStandPointIndex].rotation;
        }
        else {
            _controller.SetAutoMove(areaStandPoints[curStandPointIndex]);
        }
        
        
        curStandPointIndex++;

        if (curStandPointIndex >= areaStandPoints.Length) {
            maxFilled = true;
            curStandPointIndex = 0;
            //this.transform.localScale = new Vector3(this.transform.localScale.x + expansionVal, 1, this.transform.localScale.z + expansionVal);
        }

        Toolbox.GameplayScript.CheckAreaCompletion();

    }
}
