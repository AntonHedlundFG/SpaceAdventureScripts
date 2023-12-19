using UnityEngine;
using UnityEngine.Events;

public class ComboButtonListener : MonoBehaviour
{
    public UnityEvent OnComboEvent;

    [SerializeField] private PlayerUseButton _btnOne;
    [SerializeField] private PlayerUseButton _btnTwo;

    private bool btnOnePressed = false;
    private bool btnTwoPressed = false;

    [SerializeField] private string _audioPath;

    private void OnEnable()
    {
        _btnOne?.OnComboUseEvent.AddListener(ButtonOneEventListener);
        _btnTwo?.OnComboUseEvent.AddListener(ButtonTwoEventListener);
    }

    private void OnDisable()
    {
        _btnOne?.OnComboUseEvent.RemoveListener(ButtonOneEventListener);
        _btnTwo?.OnComboUseEvent.RemoveListener(ButtonTwoEventListener);
    }
    public void ButtonOneEventListener(bool pressed)
    {
        btnTwoPressed = pressed;
        CheckCombo();
    }
    public void ButtonTwoEventListener(bool pressed)
    {
        btnOnePressed = pressed;
        CheckCombo();
    }

    private void CheckCombo()
    {
        if (!btnOnePressed || !btnTwoPressed)
        {
            return;
        }
        
        OnComboEvent.Invoke();
        if (_audioPath != "")
        {
            FMODUnity.RuntimeManager.PlayOneShot(_audioPath);
        }

    }
}
