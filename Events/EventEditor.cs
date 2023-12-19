#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EventSO), true)]
public class EventSOEditor : Editor
{
    private EventSO _target;


    public override void OnInspectorGUI()
    {
        _target = (EventSO)target;

        base.OnInspectorGUI();

        if (GUILayout.Button("Test Invoke Event"))
        {
            _target.Event.Invoke();
        }
    }
}
#endif