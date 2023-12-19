using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Float Game Event", menuName = "Events/Float")]
public class EventSOFloat : ScriptableObject
{
    public UnityEvent<float> Event;

    public float _testInvokeValue = 0f;
}