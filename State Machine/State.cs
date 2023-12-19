using System.Collections.Generic;
using UnityEngine;


public class State : MonoBehaviour
{
    public string StateName = "New State";
    public List<Action> Actions;
    public List<StateTransition> Transitions;

    public void OnEnter(EnemyController enemyController)
    {
        foreach (Action a in Actions)
        {
            a.OnEnter(enemyController);
        }
        foreach (StateTransition st in Transitions)
        {
            st.OnEnter(enemyController);
        }
    }
    public void OnExit()
    {
        foreach (Action a in Actions)
        {
            a.OnExit();
        }
        foreach (StateTransition st in Transitions)
        {
            st.OnExit();
        }
    }

    public void OnUpdate()
    {
        foreach (Action a in Actions)
        {
            a.OnUpdate();
        }
    }
}
