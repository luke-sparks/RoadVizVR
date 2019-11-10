using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class adjustSize : MonoBehaviour
{
    public float x = 6.0f;
    public float y = 0.3f;
    public float z = 3.0f;
    public float input = 0;

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
