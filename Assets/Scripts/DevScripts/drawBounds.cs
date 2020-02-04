using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Use this to see the boundaries of the road in exact bounds.
//INSTRUCTIONS:
//   1. Drag and drop on Road
//   2. Drag in any cube to the "cover" slot
//   3. Drag the Road object into the mainRoad slot (yes I know, it's dumb,
//      But it's quick and it's dirty and it works.)

public class drawBounds : MonoBehaviour
{
    [SerializeField] private Road mainRoad;
    public GameObject cover;
    private Bounds bound;

    // Start is called before the first frame update
    void Start()
    {
        bound = mainRoad.GetRendererBounds();
    }

    // Update is called once per frame
    void Update()
    {
        cover.transform.position = mainRoad.GetRendererBounds().center;
        cover.transform.localScale = mainRoad.GetRendererBounds().size;
    }
}
