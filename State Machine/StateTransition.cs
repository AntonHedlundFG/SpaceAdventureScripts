using System.Collections.Generic;

[System.Serializable]
public class StateTransition
{
    public List<Decision> Decisions;
    public State TargetState;

    public bool CheckDecisions()
    {
        foreach (Decision decision in Decisions)
        {
            if (!decision.CheckDecision())
            {
                return false;
            }
        }
        return true;
    }
    public void OnEnter(EnemyController enemycontroller)
    {
        foreach (Decision decision in Decisions)
        {
            decision.OnEnter(enemycontroller);
        }
    }
    public void OnExit()
    {
        foreach (Decision decision in Decisions)
        {
            decision.OnExit();
        }
    }
}
