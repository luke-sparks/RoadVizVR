using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EditLaneBehavior : MonoBehaviour
{
    private const float BASE_CHANGE_FT = 0.5f;
    public Text widthText;

    public void increaseLaneWidth()
    {
        float width = float.Parse(widthText.text);
        width += BASE_CHANGE_FT;
        widthText.text = width.ToString();
        Debug.Log("Lane width increased to: " + width.ToString() + "ft.");
    }

    public void decreaseLaneWidth()
    {
        float width = float.Parse(widthText.text);
        width -= BASE_CHANGE_FT;
        widthText.text = width.ToString();
        Debug.Log("Lane width decreased to: " + width.ToString() + "ft.");
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
