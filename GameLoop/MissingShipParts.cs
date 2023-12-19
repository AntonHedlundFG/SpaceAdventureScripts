using UnityEngine;

public class MissingShipParts : MonoBehaviour
{
    [SerializeField] private GameObject _partOne;
    [SerializeField] private EventSO _partOneEvent;
    
    [SerializeField] private GameObject _partTwo;
    [SerializeField] private EventSO _partTwoEvent;

    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _wingFire;
    [SerializeField] private GameObject _boosterfire;

    private void Start()
    {
        _partOne?.SetActive(false);
        _partTwo?.SetActive(false);
    }

    private void OnEnable()
    {
        _partOneEvent?.Event.AddListener(ActivatePartOne);
        _partTwoEvent?.Event.AddListener(ActivatePartTwo);
    }

    private void OnDisable()
    {
        _partOneEvent?.Event.RemoveListener(ActivatePartOne);
        _partTwoEvent?.Event.RemoveListener(ActivatePartTwo);
    }

    private void ActivatePartOne()
    {
        _partOneEvent?.Event.RemoveListener(ActivatePartOne);
        _animator?.SetTrigger("wingTrigger");
        _wingFire.SetActive(false);
    }

    private void ActivatePartTwo()
    {
        _partTwoEvent?.Event.RemoveListener(ActivatePartTwo);
        _animator?.SetTrigger("boosterTrigger");
        _boosterfire.SetActive(false);
    }

    private void SetPartOneActive()
    {
        _partOne?.SetActive(true);        
    }

    private void SetPartTwoActive()
    {
        _partTwo?.SetActive(true);
    }


}
