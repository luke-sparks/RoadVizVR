using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(InputManager))]
public class NewBehaviourScript : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        InputManager inputManager = (InputManager)target;

        if (GUILayout.Button("Reevaluate Controller Type"))
        {
            inputManager.evaluateControllerType();
        }
    }
}
