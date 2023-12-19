using System;
using UnityEngine;
using UnityEngine.Serialization;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField] private DayNightData dayNightData;
    
    [Range(0, 1)] [SerializeField] private float _percentageOfDay;
    [Header("Set between percentage that is above Day time")] [SerializeField] [Range(0.7f , 1)] 
    private float _percentageToSpawnAgainDuringNight;
    
    [SerializeField] private float _currentTime;
    [HideInInspector] public int spawnDuringDay;
    [HideInInspector] public int spawnDuringNight;
    [HideInInspector] public int spawnDuringNightAgain;
    
    [SerializeField] private EventSO NightBegins;
    [SerializeField] private EventSO DayBegins;
    
    private float speed = 1.5f; //1.5 is 240s per day cycle. 360 / DayLengthInSeconds
    private Transform lightTransform;

    public EnemyPool _enemyPool;
    
    [SerializeField] private Material nightSkybox;
    [SerializeField] private Material daySkybox;

    private float dayTimeLength;
    
    [Header("Adjust Night Exposure")]
    [SerializeField] private float maxNightExposure = 2.5f;
    [SerializeField] private float minNightExposure = 0.15f;
    [Header("Adjust Day Exposure")]
    [SerializeField] private float maxDayExposure = 1f;
    [SerializeField] private float minDayExposure = 0.15f;
    [HideInInspector] public bool isDay = false;
    private float currentExposure;


    // [SerializeField] private Color purple;
    [Header("Change night gradient colours")]
    [SerializeField] private Color nightSky;
    [SerializeField] private Color nightEquator;
    [SerializeField] private Color nightGround;
    [Header("Change day gradient colours")]
    [SerializeField] private Color daySky;
    [SerializeField] private Color dayEquator;
    [SerializeField] private Color dayGround;
    [Header("Change fog colours")] 
    [SerializeField] private Color nightFog;
    [SerializeField] private Color dayFog;

    void Awake()
    {
        dayNightData.PercentageOfDay = _percentageOfDay;
        dayNightData.SpawnAgain = _percentageToSpawnAgainDuringNight;
        
        lightTransform = GetComponent<Transform>();
        
        dayNightData.CurrentTime = 0f;
        speed = 360 / dayNightData.DayLengthInSeconds;
        dayTimeLength = dayNightData.DayLengthInSeconds * dayNightData.PercentageOfDay; //10 * 0.7 = 7

        dayNightData.isNight = false; //Can set it to day otherwise
        dayNightData.ifSpawnAgain = false; //Can set it to day otherwise
        
        RenderSettings.skybox = daySkybox;
        isDay = false;
    }
    void Update()
    {
        _currentTime = dayNightData.CurrentTime += Time.deltaTime;
        lightTransform.RotateAround(Vector3.zero, Vector3.right, speed * Time.deltaTime);        
        
        DayNightSkybox();
    }
    private void DayNightSkybox()
    {
        float currentTime = Time.time % dayNightData.DayLengthInSeconds; //Time % 10
        float dayLerpTime = currentTime / (dayTimeLength / 2); // 7/2 = Time / 3,5

            
        if (currentTime >= dayTimeLength && isDay) // Change from day to night // 10 - 7 = 7
        {
            RenderSettings.skybox = nightSkybox;
            NightBegins?.Event.Invoke();
            isDay = false;
        }
        else if (currentTime <= dayTimeLength && !isDay) // Change from night to day //0 <= 5,6
        {
            RenderSettings.skybox = daySkybox;
            DayBegins?.Event.Invoke();
            isDay = true;
        }

        if (!isDay)
        {
            float lerpNight = (currentTime - dayTimeLength) / (dayNightData.DayLengthInSeconds - dayTimeLength); //time / (10-7)=3
            //Sky
            Color lerpNightSkyColor = Color.Lerp(nightSky, daySky, lerpNight);
            RenderSettings.ambientSkyColor = lerpNightSkyColor;
            //Equator
            Color lerpNightEquatorColor = Color.Lerp(nightEquator, dayEquator, lerpNight);
            RenderSettings.ambientEquatorColor = lerpNightEquatorColor;
            //Ground
            Color lerpNightGroundColor = Color.Lerp(nightGround, dayGround, lerpNight);
            RenderSettings.ambientGroundColor = lerpNightGroundColor;
            //Fog
            Color lerpNightFogColor = Color.Lerp(nightFog, dayFog, lerpNight);
            RenderSettings.fogColor = lerpNightFogColor;
            
            float nightLerpTime = (currentTime - dayTimeLength) / (dayNightData.DayLengthInSeconds - dayTimeLength) * 2; //Time / 6
            if (nightLerpTime <= 1f)
                currentExposure = Mathf.Lerp(minNightExposure, maxNightExposure, nightLerpTime);
            else if (nightLerpTime > 1f)
            {
                float reverseLerpTime = nightLerpTime - 1f;
                currentExposure = Mathf.Lerp(maxNightExposure, minNightExposure, reverseLerpTime);
            }
        }
        else if (isDay)
        {
            float lerpDay = currentTime / (dayTimeLength); // 7/2 = Time / 3,5
            //Sky
            Color lerpDaySkyColor = Color.Lerp(daySky, nightSky, lerpDay);
            RenderSettings.ambientSkyColor = lerpDaySkyColor;
            //Equator
            Color lerpDayEquatorColor = Color.Lerp(dayEquator, nightEquator, lerpDay);
            RenderSettings.ambientEquatorColor = lerpDayEquatorColor;
            //Ground
            Color lerpDayGroundColor = Color.Lerp(dayGround, nightGround, lerpDay);
            RenderSettings.ambientGroundColor = lerpDayGroundColor;
            //Fog
            Color lerpDayFogColor = Color.Lerp(dayFog, nightFog, lerpDay);
            RenderSettings.fogColor = lerpDayFogColor;
            
            
            if (dayLerpTime <= 1f) // Time <= 3,5
                currentExposure = Mathf.Lerp(minDayExposure, maxDayExposure, dayLerpTime);
            else if (dayLerpTime > 1f) // Time >= 3,5
            {
                float reverseLerpTime = dayLerpTime - 1f;
                currentExposure = Mathf.Lerp(maxDayExposure, minDayExposure, reverseLerpTime);
            }
        }
        RenderSettings.skybox.SetFloat("_Exposure", currentExposure);
    }
    
    
    void OnEnable()
    {
        dayNightData?.OnNightTime.AddListener(NightTime);
        dayNightData?.SpawnAgainDuringNight.AddListener(NightTimeAgain);
    }
    void OnDisable()
    {
        dayNightData?.OnNightTime.RemoveListener(NightTime);
        dayNightData?.SpawnAgainDuringNight.RemoveListener(NightTimeAgain);
    }
    private void NightTime()
    {
        _enemyPool?.SpawnEnemyAtRandomTransforms(spawnDuringNight);
    }
    private void NightTimeAgain()
    {
        _enemyPool?.SpawnEnemyAtRandomTransforms(spawnDuringNightAgain);
    }
}
