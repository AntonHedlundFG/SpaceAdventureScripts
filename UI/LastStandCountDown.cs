using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LastStandCountDown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI lastStandCounter;
    [SerializeField] private EnemyPool _pool;


    private void Awake()
    {
        lastStandCounter.gameObject.SetActive(false);
    }

    void Update()
    {
        lastStandCounter.gameObject.SetActive(_pool.startLastStand);
        lastStandCounter.SetText("Final Showdown: " + _pool.countsDown.ToString("F2"));
    }
}
