using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Int Game Event", menuName = "Events/Int")]
public class EventSOInt : ScriptableObject
{
    public UnityEvent<int> Event;

    public int _testInvokeValue = 0;
}