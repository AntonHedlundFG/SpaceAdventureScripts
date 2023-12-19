using UnityEngine;
using UnityEngine.Events;

public class EventSOListener : MonoBehaviour
{
    [SerializeField] private EventSO _listeningEvent;
    [SerializeField] private UnityEvent _triggeringEvent;

    private void OnEnable() => _listeningEvent?.Event.AddListener(TriggerEvent); 
    private void OnDisable() => _listeningEvent?.Event.RemoveListener(TriggerEvent);
    private void TriggerEvent() => _triggeringEvent?.Invoke();
}
