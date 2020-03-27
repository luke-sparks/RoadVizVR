using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacePlayer : MonoBehaviour
{

    private void LateUpdate()
    {
        var target = Camera.main.transform.position;
        target.y = transform.position.y;
        gameObject.transform.LookAt(target);
    }
}
