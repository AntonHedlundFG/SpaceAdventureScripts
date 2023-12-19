using UnityEngine;

public class EndScreenMissingShipParts : MonoBehaviour
{
    [SerializeField] private WinLoseState _winLoseState;

    [SerializeField] private GameObject _firstPart;
    [SerializeField] private GameObject _secondPart;

    private Animator _animator;

    private void Start()
    {
        if (_winLoseState == null)
        {
            return;
        }
        _firstPart?.SetActive(_winLoseState.WingOneAttached);
        _secondPart?.SetActive(_winLoseState.WingTwoAttached);

        if (TryGetComponent<Animator>(out _animator))
        {
            _animator.enabled = _winLoseState.GameWon;
        }

    }
}
