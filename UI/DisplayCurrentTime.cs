using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DisplayCurrentTime : MonoBehaviour
{
    // [SerializeField] private Text textComponent;
    [SerializeField] private TextMeshProUGUI displayCurrentTime;
    [SerializeField] private DayNightData dayNightData;
    [SerializeField] private EnemyPool _pool;
    [SerializeField] private Color dayColor;
    [SerializeField] private Color nightColor;
    private float lengthOfDay;
    private float lengthOfNight;

    private bool nightTimeReset = true;
    private bool dayTimeReset = true;

    private void Awake()
    {
        lengthOfDay = dayNightData.DayLengthInSeconds * dayNightData.PercentageOfDay;
        lengthOfNight = dayNightData.DayLengthInSeconds - lengthOfDay;
    }

    private void Update()
    {
        if (dayNightData.isNight)
        {
            dayTimeReset = true;
            if (nightTimeReset)
            {
                lengthOfNight = dayNightData.DayLengthInSeconds - dayNightData.DayLengthInSeconds * dayNightData.PercentageOfDay;
                nightTimeReset = false;
            }
                
            
            lengthOfNight -= Time.deltaTime;
            displayCurrentTime.color = nightColor;
            displayCurrentTime.SetText("Time until dawn: " + lengthOfNight.ToString("F2"));
        }
        else if (!dayNightData.isNight)
        {
            nightTimeReset = true;
            if (dayTimeReset)
            {
                lengthOfDay = dayNightData.DayLengthInSeconds * dayNightData.PercentageOfDay;
                dayTimeReset = false;
            }
                
            
            lengthOfDay -= Time.deltaTime;
            displayCurrentTime.color = dayColor;
            displayCurrentTime.SetText("Time until nightfall: " + lengthOfDay.ToString("F2"));
        }
        
        if (_pool.startLastStand)
            displayCurrentTime.gameObject.SetActive(false);
    }
}