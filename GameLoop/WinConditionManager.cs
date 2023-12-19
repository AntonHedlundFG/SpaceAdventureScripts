using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; //Do not remove

public class WinConditionManager : MonoBehaviour
{
    [SerializeField] private PlayerHealth _player1HealthObj;
    [SerializeField] private PlayerHealth _player2HealthObj;
    [SerializeField] private PlayerHealth _motherShipHealth;

    [SerializeField] private EventSO _firstWingEvent;
    [SerializeField] private EventSO _secondWingEvent;
    [SerializeField] private EventSO _winGameEvent;

    [SerializeField] private WinLoseState _winLoseState;

    private bool _p1Dead;
    private bool _p2Dead;

    [SerializeField] private Animation _loseAnim;
    [SerializeField] private Animation _winAnim;

    private Coroutine _endGameRoutine;

    private void Start()
    {
        _winLoseState.WingOneAttached = false;
        _winLoseState.WingTwoAttached = false;
        _winLoseState.GameWon = false;

        if (_winGameEvent == null && _secondWingEvent != null)
        {
            _winGameEvent = _secondWingEvent;
        }
    }

    private void OnEnable()
    {
        _firstWingEvent?.Event.AddListener(OnFirstWingHandin);
        _secondWingEvent?.Event.AddListener(OnSecondWingHandin);
        _winGameEvent?.Event.AddListener(OnWin);

        _player1HealthObj?.OnPlayerDeath.AddListener(Player1DeathCheck);
        _player2HealthObj?.OnPlayerDeath.AddListener(Player2DeathCheck);
        _motherShipHealth?.OnPlayerDeath.AddListener(MotherShipDeathCheck);
    }

    private void OnDisable()
    {
        _firstWingEvent?.Event.RemoveListener(OnFirstWingHandin);
        _secondWingEvent?.Event.RemoveListener(OnSecondWingHandin);
        _winGameEvent?.Event.RemoveListener(OnWin);

        _player1HealthObj?.OnPlayerDeath.RemoveListener(Player1DeathCheck);
        _player2HealthObj?.OnPlayerDeath.RemoveListener(Player2DeathCheck);
        _motherShipHealth?.OnPlayerDeath.RemoveListener(MotherShipDeathCheck);
    }

    private void OnFirstWingHandin() => _winLoseState.WingOneAttached = true;
    private void OnSecondWingHandin() => _winLoseState.WingTwoAttached = true;

    private void OnWin()
    {
        if (_endGameRoutine != null)
        {
            return;
        }
        _endGameRoutine = StartCoroutine(WinGame());
        _winAnim.Play();
    }

    private void Player1DeathCheck(bool isDead)
    {
        _p1Dead = isDead;
        LoseConditionCheck();
    }

    private void Player2DeathCheck(bool isDead)
    {
        _p2Dead = isDead;
        LoseConditionCheck();
    }

    private void MotherShipDeathCheck(bool isDead)
    {
        if (isDead && _endGameRoutine == null) {
            _endGameRoutine = StartCoroutine(LoseGame());
        }
    }

    private void LoseConditionCheck()
    {
        if (_p1Dead && _p2Dead && _endGameRoutine == null) {
            _endGameRoutine = StartCoroutine(LoseGame());
        }
    }
    
    private IEnumerator WinGame()
    {
        for (int i = 3; i > 0; i--) 
        {
            yield return new WaitForSeconds(1);
        }
        _winLoseState.GameWon = true;
        GoToEndScene();
    }

    private IEnumerator LoseGame()
    {
        _loseAnim.Play();
        for (int i = 3; i > 0; i--) {
            yield return new WaitForSeconds(1);
        }
        _winLoseState.GameWon = false;
        GoToEndScene();
    }

    private void GoToEndScene()
    {
        Cursor.visible = true;
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        SceneManager.LoadScene(2);
#endif
    }
}
