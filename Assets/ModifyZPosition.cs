using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyZPosition : MonoBehaviour
{
    public void modifyZPosition(int zadjust)
    {
        Vector3 currPosition = this.transform.localPosition;
        currPosition.z += zadjust;
        this.transform.localPosition.Set(currPosition.x, currPosition.y, currPosition.z);
    }
}
