using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class SetWinLoseText : MonoBehaviour
{
    [SerializeField] private WinLoseState _winLoseState;
    [SerializeField] private TMP_Text _bigText;
    [SerializeField] private TMP_Text _littleText;

    [FormerlySerializedAs("_winText")][SerializeField] private string _bigWinText;
    [FormerlySerializedAs("_loseText")] [SerializeField] private string _bigLoseText;

    [SerializeField] private string _littleWinText;
    [SerializeField] private string _littleLoseText;

    private void Start()
    {
        if (_winLoseState == null)
        {
            return;
        }

        if (_littleText != null)
        {
            _littleText.text = _winLoseState.GameWon ? _littleWinText : _littleLoseText;
        }

        if (_bigText != null)
        {
            _bigText.text = _winLoseState.GameWon ? _bigWinText : _bigLoseText;
        }
    }
}
