using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustAsphaltSize : MonoBehaviour
{
    private float x;
    private float y;
    private float z;
    private float input;
    public GameObject lane_parent_object;
    private laneBehavior lane_parent_script;



    // Start is called before the first frame update
    public void Start()
    {
        x = transform.localScale.x;
        y = transform.localScale.y;
        z = transform.localScale.z;
        lane_parent_script = lane_parent_object.GetComponent<laneBehavior>();
        input = lane_parent_script.input;
        transform.localScale = new Vector3(x, y, z);
    }

    // Update is called once per frame
    public void Update()
    {
        input = lane_parent_script.input;
        transform.localScale = new Vector3(x, y, z + input);
    }
}
