#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventSOInt), true)]
public class EventSOIntEditor : Editor
{
    private EventSOInt _target;


    public override void OnInspectorGUI()
    {
        _target = (EventSOInt)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Test Invoke Event"))
        {
            _target.Event.Invoke(_target._testInvokeValue);
        }
    }

}
#endif