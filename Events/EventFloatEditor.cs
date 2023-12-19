#if  UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventSOFloat), true)]
public class EventSOFloatEditor : Editor
{
    private EventSOFloat _target;


    public override void OnInspectorGUI()
    {
        _target = (EventSOFloat)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Test Invoke Event"))
        {
            _target.Event.Invoke(_target._testInvokeValue);
        }
    }
}
#endif