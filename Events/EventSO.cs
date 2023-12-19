using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Game Event", menuName = "Events/Parameterless")]
public class EventSO : ScriptableObject
{
    public UnityEvent Event;
}