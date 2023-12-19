using System.Collections;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "SO_DayNightData", menuName = "My Game/Day Night Data")]
public class DayNightData : ScriptableObject
{
    [SerializeField] private float _currentTime;
    [SerializeField] private float _dayLengthInSeconds = 240;
    [HideInInspector] public bool ifSpawnAgain = false;
    [HideInInspector] public bool ifContinuousSpawning = false;
    private float _percentageOfDay;
    private float _spawnAgain;
    public bool isNight = false; 

    public float SpawnAgain
    {
        get => _spawnAgain;
        set => _spawnAgain = value;
    }
    public float PercentageOfDay
    {
        get => _percentageOfDay;
        set => _percentageOfDay = value;
    }
    public float DayLengthInSeconds => _dayLengthInSeconds;

    public UnityEvent OnDayStart;
    public UnityEvent OnNightTime;
    public UnityEvent SpawnAgainDuringNight;

    public float CurrentTime
    {
        get 
        {
            return _currentTime;
        } 
        set
        {
            _currentTime = value;

            if (_currentTime >= _dayLengthInSeconds * _percentageOfDay && !isNight) //Will be night time 0.5
            {
                OnNightTime?.Invoke(); //Will spawn enemy
                ifContinuousSpawning = true;
                isNight = true;
            }

            if (_currentTime >= _dayLengthInSeconds * _spawnAgain && !ifSpawnAgain) // it will spawn again
            {
                SpawnAgainDuringNight?.Invoke();
                ifSpawnAgain = true;
            }
            
            if (_currentTime >= _dayLengthInSeconds) //Triggers after dayLength is over
            {
                _currentTime = 0f;
                OnDayStart?.Invoke();
                
                    
                ifContinuousSpawning = false;
                isNight = false;
                ifSpawnAgain = false;
            }
        }
    }
}
