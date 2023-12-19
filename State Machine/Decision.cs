using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Decision : MonoBehaviour
{
    public abstract bool CheckDecision();
    public abstract void OnEnter(EnemyController enemyController);
    public abstract void OnExit();

}
