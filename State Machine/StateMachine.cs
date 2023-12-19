using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] private EnemyController _enemyController;

    public State StartingState;

    private State _currentState;

    private void OnEnable()
    {
        _currentState = StartingState;
        _currentState.OnEnter(_enemyController);
    }
    private void Update()
    {
        foreach(StateTransition st in _currentState.Transitions)
        {
            if(st.CheckDecisions())
            {
                _currentState.OnExit();
                _currentState = st.TargetState;
                _currentState.OnEnter(_enemyController);
                break;
            }
        }

        _currentState.OnUpdate();
    }

    private void OnDisable()
    {
        _currentState.OnExit();
    }
}
