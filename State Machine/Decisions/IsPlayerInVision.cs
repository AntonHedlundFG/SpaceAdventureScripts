using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerInVision : Decision
{
    private EnemyData _enemyData;
    private EnemyController _enemyController;
    private float _fovRad;
    private List<GameObject> _players;
    private GameObject _nearestPlayer;

    public override bool CheckDecision() 
    {
        return CheckIsPlayerInVision();
    }

    private bool CheckIsPlayerInVision()
    {
        GetPlayerInRange();

        if (_nearestPlayer == null) return false;
        Vector3 dirToTarget = (_nearestPlayer.transform.position - transform.position ).normalized;
		float angleRad = AngleBetweenNormalizedVectors( transform.forward, dirToTarget );
		return angleRad < _fovRad / 2;
    }

    private float AngleBetweenNormalizedVectors( Vector3 a, Vector3 b ) 
    {
		return Mathf.Acos( Mathf.Clamp( Vector3.Dot( a, b ), -1, 1 ) );
	}

    private void GetPlayerInRange()
    {
        foreach (GameObject player in _players) {
            float dist = Vector3.Distance(player.transform.position, transform.position);
            if (dist <= _enemyData.DetectRange)
                _nearestPlayer = player;
            else 
                _nearestPlayer = null;
        }
    }

    public override void OnEnter(EnemyController enemyController)
    {
        _enemyController = enemyController;
        _enemyData = enemyController.GetEnemyDataRef();
        _fovRad = _enemyData.DetectionConeDegrees * Mathf.Deg2Rad;
        _players = _enemyController.GetPlayerRef();
    }

    public override void OnExit() 
    {
        if (_nearestPlayer != null)
            _enemyController.CurrentTarget = _nearestPlayer.transform;
    }
}
