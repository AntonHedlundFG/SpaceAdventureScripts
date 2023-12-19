using UnityEngine;
using UnityEngine.Events;

public class PlayerUseButton : MonoBehaviour
{
    public UnityEvent<bool> OnComboUseEvent;
    public UnityEvent OnSingleUseEvent;
    public UnityEvent OnSingleUseEventStop;

    [SerializeField] private string _audioPath;

    public bool IsEnabled = true;

    public virtual void PressButton()
    {
        if (!IsEnabled)
        {
            return;
        }
            OnSingleUseEvent.Invoke();
            OnComboUseEvent.Invoke(true);
            if (_audioPath != "")
            {
                FMODUnity.RuntimeManager.PlayOneShot(_audioPath);
            }
    }

    public virtual void StopPressButton()
    {
        if (!IsEnabled)
        {
            return;
        }
        OnSingleUseEventStop.Invoke();
        OnComboUseEvent.Invoke(false);
    }

    public void SetEnabled(bool state) => IsEnabled = state;
}
