using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adjustSize : MonoBehaviour
{
    public float x;
    public float y;
    public float z;
    public float input;

    // Start is called before the first frame update
    public void Start()
    {
        transform.localScale = new Vector3(x, y, z);
    }

    // Update is called once per frame
    public void Update()
    {
        transform.localScale = new Vector3(x, y, z + input);

    }
}
